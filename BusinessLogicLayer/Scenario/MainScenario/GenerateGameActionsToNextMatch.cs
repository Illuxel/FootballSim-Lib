using System;

using BusinessLogicLayer.Services;

using DatabaseLayer.Enums;
using DatabaseLayer.Repositories;
using DatabaseLayer.Services;

namespace BusinessLogicLayer.Scenario
{
    //Main Scenario. Only this scenario is public
    public class GenerateGameActionsToNextMatch
    {
        private SaveInfo _saveInfo;
        private GenerateAllMatchesByTour _generateAllMatchesByTour;
        private JuniorProcessing _juniorProcessing;
        private PlayerSkillsUpdater _playerSkillsUpdater;
        private TeamRepository _teamRepository;
        private BudgetManager _budgetManager;
        private GenerateGameActionsToNextMatchSettings _settings;

        public GenerateGameActionsToNextMatch(SaveInfo saveInfo, GenerateGameActionsToNextMatchSettings settings)
        {
            _saveInfo = saveInfo;
            _budgetManager = new BudgetManager();
            _generateAllMatchesByTour = new GenerateAllMatchesByTour(DateTime.Parse(_saveInfo.PlayerData.GameDate), _saveInfo.PlayerData.ClubId);

            _teamRepository = new TeamRepository();
            _juniorProcessing = new JuniorProcessing();
            _playerSkillsUpdater = new PlayerSkillsUpdater();
            _settings = settings;
        }

        public void SimulateActions()
        {
            //Define count of available scout requests
            resetCountOfAvailableScoutRequests();

            //Generate all matches by tour
            _generateAllMatchesByTour.Generate();
            _playerSkillsUpdater.StartTraining(_saveInfo.PlayerData.ClubId, _saveInfo.PlayerData.SelectedTrainingMode);

            var gameDate = DateTime.Parse(_saveInfo.PlayerData.GameDate);
            _budgetManager.PaySalary(gameDate);

            /*var teams = _teamRepository.Retrieve();
            //using scenario for teams
            foreach (var team in teams)
            {
                //TODO: call another scenario using date interval
            }*/


            //Increase gameDate and update real date
        }

        private void resetCountOfAvailableScoutRequests()
        {
            if (_saveInfo.PlayerData.CurrentLevel == ScoutSkillLevel.Level2)
            {
                _saveInfo.PlayerData.CountAvailableScoutRequests = 2;
            }
            else if (_saveInfo.PlayerData.CurrentLevel == ScoutSkillLevel.Level3)
            {
                _saveInfo.PlayerData.CountAvailableScoutRequests = 3;
            }
        }
    }
}
