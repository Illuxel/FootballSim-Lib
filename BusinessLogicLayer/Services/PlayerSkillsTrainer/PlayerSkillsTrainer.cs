using BusinessLogicLayer.Rules;
using BusinessLogicLayer.Services.PlayerGeneration;
using DatabaseLayer;
using DatabaseLayer.Enums;
using DatabaseLayer.Repositories;
using System;

namespace BusinessLogicLayer.Services
{
    public class PlayerSkillsTrainer
    {
        PlayerRepository _playerRepository;
        PersonRepository _personRepository;
        PlayerCoefPropertyFactory _playerCoefPropertyFactory;

        public PlayerSkillsTrainer()
        {
            _playerRepository = new PlayerRepository();
            _personRepository = new PersonRepository();
            _playerCoefPropertyFactory = new PlayerCoefPropertyFactory();
        }

        // TODO: Написати метод який буде отримувати склад команди у минулому матчі

        public void TrainPlayers(string teamId, TrainingMode trainingMode)
        {
            /*А з цим що робити
            var importancePropertyCoef = getImportancePropertyCoef(player);*/
            
            if (trainingMode == TrainingMode.SimplifiedForEveryone || trainingMode == TrainingMode.Standart || trainingMode == TrainingMode.AdvancedForEveryone)
            {
                trainAllPlayers(teamId, trainingMode);
            }
            else if(trainingMode == TrainingMode.SimplifiedForLastGamePlayers || trainingMode == TrainingMode.AdvancedForLastGameBench)
            {
                trainPlayersFromLastGame(teamId, trainingMode);
            }
        }

        private void trainAllPlayers(string teamId, TrainingMode trainingMode)
        {
            var players = _playerRepository.Retrieve(teamId);
            var trainingCoeff = getTrainingCoeff(trainingMode);

            foreach (var player in players)
            {
                var ageCoeff = getAgeCoeff(player);
                var percent = calculatePercent(ageCoeff, trainingCoeff);
                
                if (isWillBeImproved(percent))
                {
                    improveRandomSkill(player);
                }

                _playerRepository.Update(player);
            }
        }
        private void trainPlayersFromLastGame(string teamId, TrainingMode trainingMode)
        {
            
        }
        private double getAgeCoeff(Player player)
        {
            var personId = player.PersonID;
            var person = _personRepository.Retrieve(personId);
            var age = DateTime.Now.Year - person.Birthday.Year;

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

        private double calculatePercent(double ageCoeff,double trainingCoeff)
        {
            return ageCoeff * trainingCoeff;
        }

        private PlayerCoefImportanceProperty getImportancePropertyCoef(Player player)
        {
            var position = EnumDescription.GetEnumValueFromDescription<PlayerPosition>(player.PositionCode);
            return _playerCoefPropertyFactory.Create(position);
        }

        private bool isWillBeImproved(double percent)
        {
            var randomPercent = new Random().Next(0,100);
            return randomPercent <= percent * 100;
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
    }
}
