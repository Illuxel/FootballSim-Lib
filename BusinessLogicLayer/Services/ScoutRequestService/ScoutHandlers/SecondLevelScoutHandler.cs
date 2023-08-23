using DatabaseLayer.Services;
using System;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services.ScoutRequestService
{
    internal class SecondLevelScoutHandler : FirstLevelScoutHandler
    {
        public SecondLevelScoutHandler(PlayerGameData playerGameData) : base(playerGameData)
        {}

        public override int LookForAverageRatingInJuniorAcademy(string teamId)
        { 
            var leagueId = defineLeagueId(teamId);
            var leagueIds = defineValidLeagueIds(leagueId);

            var IsTeamInLeague = _leagueRepository.IsTeamInLeague(teamId, leagueIds);
            if (IsTeamInLeague)
            {
                return _juniorFinder.AverageJuniorRatingByTeam(teamId);
            }
            return 0;
        }

        public virtual int LookForWorstPlayerInJuniorAcademy(string teamId)
        {
            if(isCountOfRequestsPerWeekIsNotExceeded())
            {
                var leagueId = defineLeagueId(teamId);
                var leagueIds = defineValidLeagueIds(leagueId);

                var IsTeamInLeague = _leagueRepository.IsTeamInLeague(teamId, leagueIds);
                if (IsTeamInLeague)
                {
                    _playerGameData.CountOfRequestsPerTime++;
                    var worstPlayer = _juniorFinder.WorstJuniorPlayerByTeam(teamId);
                    if (worstPlayer != null)
                    {
                        return _juniorFinder.WorstJuniorPlayerByTeam(teamId).Rating;
                    }
                    return 0;
                }
            }

            
            return 0;
        }

        protected override List<int> defineValidLeagueIds(int leagueId)
        {
            List<int> leagueIds;
            if (leagueId != 1 && leagueId != maxLeagueId)
            {
                leagueIds = new List<int>() { leagueId - 1, leagueId, leagueId + 1 };
            }
            else if (leagueId == 1)
            {
                leagueIds = new List<int>() { maxLeagueId, leagueId, leagueId + 1 };
            }
            else
            {
                leagueIds = new List<int>() { leagueId - 1, leagueId, 1 };
            }
            return leagueIds;
        }

        protected virtual int _countOfRequestsPerTime
        {
            get => 2;
        }

        protected bool isCountOfRequestsPerWeekIsNotExceeded()
        {
            if(_playerGameData.CountOfRequestsPerTime == 0)
            {
                _playerGameData.FirstRequstDate = DateTime.Parse(_playerGameData.GameDate);
                _playerGameData.EndOfLimitsDate = DateTime.Parse(_playerGameData.GameDate).AddDays(7);
                return true;
            }
            else if(DateTime.Parse(_playerGameData.GameDate) > _playerGameData.EndOfLimitsDate)
            {
                _playerGameData.CountOfRequestsPerTime = 0;
                _playerGameData.FirstRequstDate = DateTime.Parse(_playerGameData.GameDate);
                _playerGameData.EndOfLimitsDate = DateTime.Parse(_playerGameData.GameDate).AddDays(7);
                return true;
            }
            else if(DateTime.Parse(_playerGameData.GameDate) >= _playerGameData.FirstRequstDate 
                && DateTime.Parse(_playerGameData.GameDate) <= _playerGameData.EndOfLimitsDate)
            {
                return _playerGameData.CountOfRequestsPerTime < _countOfRequestsPerTime;
            }

            return false;
        }
    }
}
