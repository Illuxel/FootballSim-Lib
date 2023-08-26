using DatabaseLayer.Repositories;
using System;

namespace BusinessLogicLayer.Scenario
{
    //Main Scenario. Only this scenario is public
    public class GenerateGameActionsToNextMatch
    {
        private GenerateAllMatchesByTour _generateAllMatchesByTour;
        private JuniorGeneration _juniorGeneration;
        private PlayerSkillsUpdater _playerSkillsUpdater;
        private TeamRepository _teamRepository;
        private string _ownerTeamId;
        public GenerateGameActionsToNextMatch(DateTime gameDate, string ownerTeamId) 
        {
            _generateAllMatchesByTour = new GenerateAllMatchesByTour(gameDate, ownerTeamId);
            _teamRepository = new TeamRepository();
            _juniorGeneration = new JuniorGeneration();
            _playerSkillsUpdater = new PlayerSkillsUpdater();
            _ownerTeamId = ownerTeamId;
        }

        public void SimulateActions()
        {
            _generateAllMatchesByTour.Generate();

            /*var teams = _teamRepository.Retrieve();
            //using scenario for teams
            foreach (var team in teams)
            {
                //TODO: call another scenario using date interval
            }*/
        }
    }
}
