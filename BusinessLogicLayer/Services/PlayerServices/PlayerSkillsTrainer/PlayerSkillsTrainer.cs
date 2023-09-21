using BusinessLogicLayer.Rules;
using BusinessLogicLayer.Services.PlayerServices;
using DatabaseLayer;
using DatabaseLayer.Enums;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
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

        public delegate void PlayerGrowUpHandler(Player player);
        public event PlayerGrowUpHandler? OnPlayerGrowUp;

        public delegate void PlayerGrowDownHandler(Player player);
        public event PlayerGrowDownHandler? OnPlayerGrowDown;

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

        public List<IPlayerTrainingPreview> GetPlayersPreview(string teamId, TrainingMode trainingMode)
        {
            var players = _playerRepository.Retrieve(teamId);
            var playerPreview = new List<IPlayerTrainingPreview>();
            var defaultEnduranceCost = defineEnduranceCost(trainingMode);
            var enduranceCostForLastGamePlayers = defineEnduranceCostForLastGamePlayers(trainingMode);

            foreach (var player in players)
            {
                //If player is not injuried, we can train him and we don't show him in preview
                if (!_playerInjuryFinder.IsAlreadyInjuried(player))
                {
                    int enduranceCostPercentForPlayer = defaultEnduranceCost;

                    if (isPlayerHaveSpecificEnduranceCost(player, teamId, trainingMode))
                    {
                        enduranceCostPercentForPlayer = enduranceCostForLastGamePlayers;
                    }

                    var enduranceAfterTrain = calculateEnduranceAfterTraining(player, enduranceCostPercentForPlayer);

                    //If not enough endurance, player can't train, so we don't show him in preview
                    if (enduranceAfterTrain >= 0)
                    {
                        var playerData = _personRepository.Retrieve(player.PersonID);
                        var playerName = playerData.Name == null ? playerData.Surname : $"{playerData.Name} {playerData.Surname}";

                        var preview = new PlayerTrainingPreview()
                        {
                            PersonID = player.PersonID,
                            Name = playerName,
                            Position = player.Position,
                            Overall = player.Rating,
                            CurrentEndurance = player.Endurance,
                            AfterTrainEndurance = enduranceAfterTrain
                            //Maybee we need to add here TrainingMode field, `cuz in TrainPlayers(List<IPlayerTrainingPreview>) method we need to know which training mode we use
                        };

                        playerPreview.Add(preview);
                    }
                }
            }

            return playerPreview;
        }
        public void TrainPlayers(string teamId, List<IPlayerTrainingPreview> playersPreviews, TrainingMode trainingMode)
        {
            var players = _playerRepository.Retrieve(teamId);
            var trainingCoeff = getTrainingCoeff(trainingMode);
            var gameDate = defineDate(teamId);

            var trainedPlayers = getTrainedPlayers(playersPreviews, players, trainingCoeff, gameDate);

            _playerRepository.Update(trainedPlayers);
        }
    
        public void TrainPlayers(string teamId, TrainingMode trainingMode)
        {
            var playersPreviews = GetPlayersPreview(teamId, trainingMode);
            var players = _playerRepository.Retrieve(teamId);
            var trainingCoeff = getTrainingCoeff(trainingMode);
            var gameDate = defineDate(teamId);

            var trainedPlayers = getTrainedPlayers(playersPreviews, players, trainingCoeff, gameDate);

            _playerRepository.Update(trainedPlayers);
        }

        private List<Player> getTrainedPlayers(List<IPlayerTrainingPreview> playersPreviews, List<Player> players, double trainingCoeff, DateTime gameDate)
        {
            var trainedPlayers = new List<Player>();
            foreach (var playerPreview in playersPreviews)
            {
                var player = players.FirstOrDefault(x => x.PersonID == playerPreview.PersonID);

                if (player != null)
                {
                    defineAge(player, gameDate);

                    var ageCoeff = getAgeCoeff();
                    var percent = calculatePercent(ageCoeff, trainingCoeff);

                    if (_playerInjuryFinder.IsInjuried(player))
                    {
                        _playerInjuryFinder.SetInjury(player, gameDate);
                    }
                    else if (processChance(percent))
                    {
                        player = successTrain(player, playerPreview.AfterTrainEndurance);
                    }
                    else if (isOldPlayer())
                    {
                        var decreaseCoef = oldPlayerDecreaseCoeff();

                        if (processChance(decreaseCoef))
                        {
                            player = decreaseStatsTraining(player);
                        }
                    }
                    trainedPlayers.Add(player);
                }
            }
            return trainedPlayers;
        }

        private Player successTrain(Player player, int afterTrainEndurance)
        {
            changeStats(player);
            var oldRating = player.Rating;
            player.Rating = calculatePlayerStats(player);
            player.Endurance = afterTrainEndurance;
            if (oldRating != player.Rating)
            {
                OnPlayerGrowUp?.Invoke(player);
            }
            return player;
        }
        private Player decreaseStatsTraining(Player player)
        {
            changeStats(player, false);
            var oldRating = player.Rating;
            player.Rating = calculatePlayerStats(player);
            if (oldRating != player.Rating)
            {
                OnPlayerGrowDown?.Invoke(player);
            }
            return player;
        }
        private bool isPlayerHaveSpecificEnduranceCost(Player player,string teamId,TrainingMode trainingMode)
        {
            if (trainingMode == TrainingMode.SimplifiedForLastGamePlayers || trainingMode == TrainingMode.AdvancedForLastGameBench)
            {
                if (isPlayerInLastGame(teamId, player.PersonID))
                {
                    return true;
                }
            }

            return false;
        }

        private double oldPlayerDecreaseCoeff()
        {
            if (_currentPlayerAge >= 31 && _currentPlayerAge <= 32)
            {
                return 0.05;
            }
            else if (_currentPlayerAge >= 33 && _currentPlayerAge < 34)
            {
                return 0.15;
            }
            else if (_currentPlayerAge >= 35 && _currentPlayerAge < 36)
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

        private bool isPlayerInLastGame(string teamId, string playerId)
        {
            return _playerInvolvementLastMatchChecker.Check(teamId, playerId) == true;
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

        private int calculateEnduranceAfterTraining(Player player, int enduranceCostPercent)
        {
            var enduranceCost = player.Endurance * enduranceCostPercent / 100;
            return player.Endurance -= enduranceCost;
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
        private int defineAge(Player player, DateTime gameDate)
        {
            var personId = player.PersonID;
            var person = _personRepository.Retrieve(personId);
            _currentPlayerAge = gameDate.Year - person.Birthday.Year;
            return _currentPlayerAge;
        }
        private void changeStats(Player player, bool isUpgade = true)
        {
            int updatingValue = isUpgade ? 1 : -1;
            var randomSkill = new Random().Next(0, 5);

            switch (randomSkill)
            {
                case 0:
                    player.Speed += updatingValue;
                    break;
                case 1:
                    player.Strike += updatingValue;
                    break;
                case 2:
                    player.Physics += updatingValue;
                    break;
                case 3:
                    player.Defending += updatingValue;
                    break;
                case 4:
                    player.Passing += updatingValue;
                    break;
                case 5:
                    player.Dribbling += updatingValue;
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
