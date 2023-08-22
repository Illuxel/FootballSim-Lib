using DatabaseLayer.Services;

namespace BusinessLogicLayer.Services.ScoutRequestService
{
    internal class ThirdLevelScoutHandler : SecondLevelScoutHandler
    {
        public ThirdLevelScoutHandler(PlayerGameData playerGameData) : base(playerGameData)
        {}

        public override int LookForAverageRatingInJuniorAcademy(string teamId)
        {
            return base.LookForAverageRatingInJuniorAcademy(teamId);
        }

        public override int LookForWorstPlayerInJuniorAcademy(string teamId)
        {
            return base.LookForWorstPlayerInJuniorAcademy(teamId);
        }

        public virtual int LookForBestPlayerInJuniorAcademy(string teamId)
        {
            var leagueId = defineLeagueId(teamId);
            var leagueIds = defineValidLeagueIds(leagueId);

            var IsTeamInLeague = _leagueRepository.IsTeamInLeague(teamId, leagueIds);
            if (IsTeamInLeague)
            {
                return _juniorFinder.BestJuniorPlayerByTeam(teamId).Rating;
            }
            return 0;
        }

        protected override int defineCountOfRequestsPerWeek()
        {
            return 3;
        }
    }
}
