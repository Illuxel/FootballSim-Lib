using BusinessLogicLayer.Services.ScoutRequestService;
using DatabaseLayer.Services;
using DatabaseLayer.Enums;
using System;

namespace BusinessLogicLayer.Services
{
    public class ScoutRequest
    {
        PlayerGameData _playerGameData;
        dynamic _scoutHandler;
        public ScoutRequest(PlayerGameData playerGameData)
        {
            _playerGameData = playerGameData;
        }

        private dynamic defineScoutHandler()
        {
            if(_playerGameData.CurrentLevel == ScoutSkillLevel.Level1)
            {
                return new FirstLevelScoutHandler(_playerGameData);
            }
            else if(_playerGameData.CurrentLevel == ScoutSkillLevel.Level2)
            {
                return new SecondLevelScoutHandler(_playerGameData);
            }
            else if(_playerGameData.CurrentLevel == ScoutSkillLevel.Level3)
            {
                return new ThirdLevelScoutHandler(_playerGameData);
            }
            else
            {
                throw new Exception("Scout has reached the max level");
            }
        }
        private void setHandler()
        {
            if (_scoutHandler == null)
            {
                _scoutHandler = defineScoutHandler();
            }
        }
        public int LookForAverageRatingInJuniorAcademy(string teamId)
        {
            setHandler();

            return _scoutHandler.LookForAverageRatingInJuniorAcademy(teamId);
        }
        public int LookForWorstPlayerInJuniorAcademy(string teamId)
        {
            setHandler();

            if (_playerGameData.CurrentLevel > ScoutSkillLevel.Level1)
            {
                return _scoutHandler.LookForWorstPlayerInJuniorAcademy(teamId);
            }
            return 0;
        }
        public int LookForBestPlayerInJuniorAcademy(string teamId)
        {
            setHandler();

            if (_playerGameData.CurrentLevel > ScoutSkillLevel.Level2)
            {
                return _scoutHandler.LookForBestPlayerInJuniorAcademy(teamId);
            }
            return 0;
        }
    }
}
