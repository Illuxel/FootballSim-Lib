using BusinessLogicLayer.Services;
using DatabaseLayer.Enums;
using DatabaseLayer.Repositories;
using DatabaseLayer.Services;
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
        private JuniorGeneration _juniorGeneration;
        private PlayerSkillsUpdater _playerSkillsUpdater;
        private TeamRepository _teamRepository;
        private TeamPositionCalculator _teamPositionCalculator;
        private SeasonValueCreator _seasonValueCreator;

        public GenerateGameActionsToNextMatch(SaveInfo saveInfo) 
        {
            _teamRepository = new TeamRepository();
            _juniorGeneration = new JuniorGeneration();
            _playerSkillsUpdater = new PlayerSkillsUpdater();
            _teamPositionCalculator = new TeamPositionCalculator();
            _seasonValueCreator = new SeasonValueCreator();

            _saveInfo = saveInfo;
            _gameDate = defineGameDate(saveInfo);
            _season = _seasonValueCreator.GetSeason(_gameDate);
            _generateAllMatchesByTour = new GenerateAllMatchesByTour(_gameDate, _saveInfo.PlayerData.ClubId);
        }

        public void SimulateActions()
        {
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
