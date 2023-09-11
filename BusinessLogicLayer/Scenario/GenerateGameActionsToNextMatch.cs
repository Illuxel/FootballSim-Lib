using DatabaseLayer.Enums;
using DatabaseLayer.Repositories;
using DatabaseLayer.Services;
using System;

namespace BusinessLogicLayer.Scenario
{
    //Main Scenario. Only this scenario is public
    public class GenerateGameActionsToNextMatch
    {
        private SaveInfo _saveInfo;
        private GenerateAllMatchesByTour _generateAllMatchesByTour;
        private JuniorGeneration _juniorGeneration;
        private PlayerSkillsUpdater _playerSkillsUpdater;
        private TeamRepository _teamRepository;

        public GenerateGameActionsToNextMatch(SaveInfo saveInfo) 
        {
            _saveInfo = saveInfo;
            _generateAllMatchesByTour = new GenerateAllMatchesByTour(DateTime.Parse(_saveInfo.PlayerData.GameDate));

            _teamRepository = new TeamRepository();
            _juniorGeneration = new JuniorGeneration();
            _playerSkillsUpdater = new PlayerSkillsUpdater();
        }

        public void SimulateActions()
        {
            //Define count of available scout requests
            resetCountOfAvailableScoutRequests();

            //Generate all matches by tour
            _generateAllMatchesByTour.Generate();


            /*var teams = _teamRepository.Retrieve();
            //using scenario for teams
            foreach (var team in teams)
            {
                //TODO: call another scenario using date interval
            }*/
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
