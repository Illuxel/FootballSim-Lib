using DatabaseLayer.Enums;
using DatabaseLayer.Repositories;
using DatabaseLayer.Services;
using BusinessLogicLayer.Services;
using System;
using System.Globalization;

namespace BusinessLogicLayer.Scenario
{
    //Main Scenario. Only this scenario is public
    public class GenerateGameActionsToNextMatch
    {
        private SaveInfo _saveInfo;
        private DateTime _gameDate;
        private string _season;
        private GenerateAllMatchesByTour _generateAllMatchesByTour;
        private JuniorProcessing _juniorProcessing;
        private PlayerSkillsUpdater _playerSkillsUpdater;
        private TeamRepository _teamRepository;
        private SponsorContractRequestor _sponsorContractRequestor;
        private GenerateGameActionsToNextMatchSettings _settings;
        private TeamPositionCalculator _teamPositionCalculator;
        private SeasonValueCreator _seasonValueCreator;

        private static DateTime _firstSeason;

        public GenerateGameActionsToNextMatch(SaveInfo saveInfo, GenerateGameActionsToNextMatchSettings settings)
        {
            _teamRepository = new TeamRepository();
            _juniorProcessing = new JuniorProcessing();
            _playerSkillsUpdater = new PlayerSkillsUpdater();
            _sponsorContractRequestor = new SponsorContractRequestor();
            _settings = settings;
            _teamPositionCalculator = new TeamPositionCalculator();
            _seasonValueCreator = new SeasonValueCreator();

            _saveInfo = saveInfo;
            _gameDate = defineGameDate(saveInfo);
            _season = _seasonValueCreator.GetSeason(_gameDate);
            _generateAllMatchesByTour = new GenerateAllMatchesByTour(_gameDate, _saveInfo.PlayerData.ClubId);
        }

        public void SimulateActions()
        {
            updateSeasonContracts();

            //Define count of available scout requests
            resetCountOfAvailableScoutRequests();

            //Generate all matches by tour
            _generateAllMatchesByTour.Generate();

            _teamPositionCalculator.CalculatePosition(_season);

            _playerSkillsUpdater.StartTraining(_saveInfo.PlayerData.ClubId, _saveInfo.PlayerData.SelectedTrainingMode);

            
            /* _gameDate.AddDays(7);
            _saveInfo.PlayerData.GameDate = _gameDate.ToString("yyyy-MM-dd");
            LoadGameManager.GetInstance().SaveGame(_saveInfo);*/


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

            if (presentSeason != fisrtSeason)
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

        private DateTime defineGameDate(SaveInfo save)
        {
            var format = "yyyy-MM-dd";
            DateTime dateTime;

            if (DateTime.TryParseExact(save.PlayerData.GameDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }

            return DateTime.MinValue;
        }
    }
}
