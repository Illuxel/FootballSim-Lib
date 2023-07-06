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

        int _currentPlayerAge;

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

                defineAge(player, gameDate);
                
                var ageCoeff = getAgeCoeff();
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
                        _playerInjuryFinder.SetInjury(player, gameDate);
                    }
                    else if (processChance(percent))
                    {
                        improveRandomSkill(player);
                        var oldRating = player.Rating; 
                        player.Rating = calculatePlayerStats(player);
                        if(oldRating != player.Rating)
                        {
                            //event
                        }
                    }
                    if(isOldPlayer())
                    {
                        var decreaseCoef = oldPlayerDecreaseCoeff();
                        if(processChance(decreaseCoef))
                        {
                            decreaseRandomSkill(player);
                            var oldRating = player.Rating;
                            player.Rating = calculatePlayerStats(player);
                            if (oldRating != player.Rating)
                            {
                                //event
                            }
                        }
                    }
                    _playerRepository.Update(player);
                }
            }
        }
        
        private double oldPlayerDecreaseCoeff()
        {
            if(_currentPlayerAge >= 31 && _currentPlayerAge <= 32)
            {
                return 0.05;
            }
            else if(_currentPlayerAge >= 33 && _currentPlayerAge < 34)
            {
                return 0.15;
            }
            else if(_currentPlayerAge >= 35 && _currentPlayerAge < 36)
            {
                return 0.25;
            }
            else
            {
                return 0.50;
            }
        }

        private bool isOldPlayer()
        {
            return _currentPlayerAge > 30;
        }

        private bool isPlayerInLastGame(string teamId,string playerId)
        {
            return _playerInvolvementLastMatchChecker.Check(teamId,playerId) == true;
        }

        private bool processChance(double percent)
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

        private double getAgeCoeff()
        {
            if (_currentPlayerAge <= 21)
            {
                return 2;
            }
            else if (_currentPlayerAge >= 22 && _currentPlayerAge <= 27)
            {
                return 1;
            }
            else if (_currentPlayerAge >= 28 && _currentPlayerAge <= 31)
            {
                return 0.5;
            }

            return 0.05;
        }
        private int defineAge(Player player,DateTime gameDate)
        {
            var personId = player.PersonID;
            var person = _personRepository.Retrieve(personId);
            _currentPlayerAge = gameDate.Year - person.Birthday.Year;
            return _currentPlayerAge;
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
        private void decreaseRandomSkill(Player player)
        {
            var randomSkill = new Random().Next(0, 5);

            switch (randomSkill)
            {
                case 0:
                    player.Speed--;
                    break;
                case 1:
                    player.Strike--;
                    break;
                case 2:
                    player.Physics--;
                    break;
                case 3:
                    player.Defending--;
                    break;
                case 4:
                    player.Passing--;
                    break;
                case 5:
                    player.Dribbling--;
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
