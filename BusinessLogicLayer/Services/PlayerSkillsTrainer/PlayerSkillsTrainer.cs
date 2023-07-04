using BusinessLogicLayer.Rules;
using DatabaseLayer;
using DatabaseLayer.Enums;
using DatabaseLayer.Repositories;
using System;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class PlayerSkillsTrainer
    {
        PlayerRepository _playerRepository;
        PersonRepository _personRepository;
        MatchRepository _matchRepository;
        PlayerCoefPropertyFactory _playerCoefPropertyFactory;
        PlayerInjuryFinder _playerInjuryFinder;
        PlayerGeneration _playerGeneration;
        PlayerInvolvementLastMatchChecker _playerInvolvementLastMatchChecker;

        public PlayerSkillsTrainer()
        {
            _playerRepository = new PlayerRepository();
            _personRepository = new PersonRepository();
            _matchRepository = new MatchRepository();
            _playerCoefPropertyFactory = new PlayerCoefPropertyFactory();
            _playerInjuryFinder = new PlayerInjuryFinder();
            _playerGeneration = new PlayerGeneration();
            _playerInvolvementLastMatchChecker = new PlayerInvolvementLastMatchChecker();
        }

        public void TrainPlayers(string teamId, TrainingMode trainingMode)
        {
            var gameDate = defineDate(teamId);
            var players = _playerRepository.Retrieve(teamId);
            var enduranceCostPercent = defineEnduranceCost(trainingMode);
            var enduranceCostPercentForPlayersInLastMatch = defineEnduranceCostForLastGamePlayers(trainingMode);

            var trainingCoeff = getTrainingCoeff(trainingMode);
            bool isEnoughEndurance;


            foreach (var player in players)
            {
                if (_playerInjuryFinder.IsAlreadyInjuried(player))
                {
                    break;
                }

                var ageCoeff = getAgeCoeff(player,gameDate);
                var percent = calculatePercent(ageCoeff, trainingCoeff);

                if (trainingMode == TrainingMode.SimplifiedForLastGamePlayers || trainingMode == TrainingMode.AdvancedForLastGameBench)
                {
                    if (isPlayerInLastGame(teamId, player.PersonID))
                    {
                        isEnoughEndurance = isEnoughEnduranceCost(player, enduranceCostPercentForPlayersInLastMatch);
                    }
                    else
                    {
                        isEnoughEndurance = isEnoughEnduranceCost(player, enduranceCostPercent);
                    }
                }
                else
                {
                    isEnoughEndurance = isEnoughEnduranceCost(player, enduranceCostPercent);
                }

                if(isEnoughEndurance)
                {
                    if (_playerInjuryFinder.IsInjuried(player))
                    {
                        Console.WriteLine("IS INJURIED");
                        _playerInjuryFinder.SetInjury(player, gameDate);
                    }
                    else if (isWillBeImproved(percent))
                    {
                        Console.WriteLine("IS IMPROVE");
                        improveRandomSkill(player);
                        player.Rating = calculatePlayerStats(player);
                    }

                    _playerRepository.Update(player);
                }
            }
        }
        
        private bool isPlayerInLastGame(string teamId,string playerId)
        {
            return _playerInvolvementLastMatchChecker.Check(teamId,playerId) == true;
        }

        private bool isWillBeImproved(double percent)
        {
            var randomPercent = new Random().Next(0, 100);
            return randomPercent <= percent * 100;
        }

        private int calculatePlayerStats(Player player)
        {
            var importancePropertyCoef = getImportancePropertyCoef(player);
            return _playerGeneration.CalculateAverageRating(player, importancePropertyCoef);
        }

        private bool isEnoughEnduranceCost(Player player, int enduranceCostPercent)
        {
            var enduranceCost = player.Endurance * enduranceCostPercent / 100;
            var restEndurance = player.Endurance -= enduranceCost;

            if (restEndurance >= 0)
            {
                player.Endurance = restEndurance;
                return true;
            }
            return false;
        }

        private DateTime defineDate(string teamId)
        {
            var matches = _matchRepository.Retrieve(teamId);
            var lastPlayedTour = matches.Where(x => x.IsPlayed).Max(x => x.TourNumber);
            var stringDate = matches.Where(x => x.TourNumber == lastPlayedTour).FirstOrDefault();
            return DateTime.Parse(stringDate.MatchDate);
        }

        private int defineEnduranceCost(TrainingMode trainingMode)
        {
            return trainingMode switch
            {
                TrainingMode.SimplifiedForEveryone => 10,
                TrainingMode.SimplifiedForLastGamePlayers => 15,
                TrainingMode.Standart => 15,
                TrainingMode.AdvancedForLastGameBench => 25,
                TrainingMode.AdvancedForEveryone => 25,
                _ => 0
            };
        }

        private int defineEnduranceCostForLastGamePlayers(TrainingMode trainingMode)
        {
            return trainingMode switch
            {
                TrainingMode.SimplifiedForLastGamePlayers => 10,
                TrainingMode.AdvancedForLastGameBench => 15,
                _ => 0
            };
        }
        
        private double getTrainingCoeff(TrainingMode trainingMode)
        {
            return trainingMode switch
            {
                TrainingMode.SimplifiedForEveryone => 0.1,
                TrainingMode.SimplifiedForLastGamePlayers => 0.1,
                TrainingMode.Standart => 0.15,
                TrainingMode.AdvancedForLastGameBench => 0.25,
                TrainingMode.AdvancedForEveryone => 0.25,
                _ => 0
            };
        }

        private double getAgeCoeff(Player player, DateTime gameDate)
        {
            var personId = player.PersonID;
            var person = _personRepository.Retrieve(personId);
            var age = gameDate.Year - person.Birthday.Year;

            if (age <= 21)
            {
                return 2;
            }
            else if (age >= 22 && age <= 27)
            {
                return 1;
            }
            else if (age >= 28 && age <= 31)
            {
                return 0.5;
            }

            return 0.05;
        }

        private void improveRandomSkill(Player player)
        {
            var randomSkill = new Random().Next(0, 5);

            switch (randomSkill)
            {
                case 0:
                    player.Speed++;
                    break;
                case 1:
                    player.Strike++;
                    break;
                case 2:
                    player.Physics++;
                    break;
                case 3:
                    player.Defending++;
                    break;
                case 4:
                    player.Passing++;
                    break;
                case 5:
                    player.Dribbling++;
                    break;

                default:
                    break;
            }
        }

        private PlayerCoefImportanceProperty getImportancePropertyCoef(Player player)
        {
            var position = EnumDescription.GetEnumValueFromDescription<PlayerPosition>(player.PositionCode);
            return _playerCoefPropertyFactory.Create(position);
        }

        private double calculatePercent(double ageCoeff, double trainingCoeff)
        {
            return ageCoeff * trainingCoeff;
        }
    }
}
