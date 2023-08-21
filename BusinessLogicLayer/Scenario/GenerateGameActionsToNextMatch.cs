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
        public GenerateGameActionsToNextMatch(DateTime gameDate) 
        {
            _generateAllMatchesByTour = new GenerateAllMatchesByTour(gameDate);
            _teamRepository = new TeamRepository();
            _juniorGeneration = new JuniorGeneration();
            _playerSkillsUpdater = new PlayerSkillsUpdater();
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
