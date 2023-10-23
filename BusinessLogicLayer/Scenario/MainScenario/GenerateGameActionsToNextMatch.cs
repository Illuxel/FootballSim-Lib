using BusinessLogicLayer.Services;

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
        private JuniorProcessing _juniorProcessing;
        private PlayerSkillsUpdater _playerSkillsUpdater;
        private TeamRepository _teamRepository;
        private SponsorContractRequestor _sponsorContractRequestor;
        private GenerateGameActionsToNextMatchSettings _settings;

        private static DateTime _firstSeason;

        public GenerateGameActionsToNextMatch(SaveInfo saveInfo, GenerateGameActionsToNextMatchSettings settings)
        {
            _saveInfo = saveInfo;
            _generateAllMatchesByTour = new GenerateAllMatchesByTour(DateTime.Parse(_saveInfo.PlayerData.GameDate), _saveInfo.PlayerData.ClubId);
            _teamRepository = new TeamRepository();
            _juniorProcessing = new JuniorProcessing();
            _playerSkillsUpdater = new PlayerSkillsUpdater();
            _sponsorContractRequestor = new SponsorContractRequestor();
            _settings = settings;
        }

        public void SimulateActions()
        {
            updateSeasonContracts();

            //Define count of available scout requests
            resetCountOfAvailableScoutRequests();

            //Generate all matches by tour
            _generateAllMatchesByTour.Generate();
            _playerSkillsUpdater.StartTraining(_saveInfo.PlayerData.ClubId, _saveInfo.PlayerData.SelectedTrainingMode);

            /*var teams = _teamRepository.Retrieve();
            //using scenario for teams
            foreach (var team in teams)
            {
                //TODO: call another scenario using date interval
            }*/

            //Increase gameDate and update real date
        }

        private void updateSeasonContracts()
        {
            var gameDate = DateTime.Parse(_saveInfo.PlayerData.GameDate);

            if (_firstSeason == DateTime.MinValue)
            {
                _firstSeason = gameDate;
            }

            var seasonCreator = new SeasonValueCreator();
            var presentSeason = seasonCreator.GetSeason(gameDate);
            var fisrtSeason = seasonCreator.GetSeason(_firstSeason);

            if (presentSeason == fisrtSeason)
            {
                return;
            }

            new RatingActualizer().Actualize(gameDate);

            foreach (var team in _teamRepository.Retrieve())
            {
                var contracts = _sponsorContractRequestor.CreateContractRequests(team.Id, gameDate.Year);

                foreach (var contract in contracts)
                {
                    team.Budget += contract.Value;
                    _sponsorContractRequestor.ClaimContract(team.Id, contract);
                }
            }
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
