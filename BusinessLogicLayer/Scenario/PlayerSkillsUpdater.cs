using BusinessLogicLayer.Services;
using DatabaseLayer.Enums;

namespace BusinessLogicLayer.Scenario
{
    // Сценарій має запускатись після кожного матчу
    internal class PlayerSkillsUpdater
    {
        PlayerSkillsTrainer _playerSkillsTrainer;
        public PlayerSkillsUpdater()
        {
            _playerSkillsTrainer = new PlayerSkillsTrainer();
        }
        public void StartTraining(string teamId,TrainingMode trainingMode)
        {
            _playerSkillsTrainer.TrainPlayers(teamId,trainingMode);
        }
    }
}
