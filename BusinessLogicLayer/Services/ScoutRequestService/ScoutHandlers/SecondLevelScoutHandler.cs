using DatabaseLayer.Services;
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
            var leagueId = defineLeagueId(teamId);
            var leagueIds = defineValidLeagueIds(leagueId);

            var IsTeamInLeague = _leagueRepository.IsTeamInLeague(teamId, leagueIds);
            if (IsTeamInLeague)
            {
                return _juniorFinder.WorstJuniorPlayerByTeam(teamId).Rating;
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

        protected virtual int defineCountOfRequestsPerWeek()
        {
            return 2;
        }
    }
}
