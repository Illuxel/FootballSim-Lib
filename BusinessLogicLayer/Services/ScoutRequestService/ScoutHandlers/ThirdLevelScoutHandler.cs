using DatabaseLayer.Services;

namespace BusinessLogicLayer.Services.ScoutRequestService
{
    internal class ThirdLevelScoutHandler : SecondLevelScoutHandler
    {
        public ThirdLevelScoutHandler(PlayerGameData playerGameData) : base(playerGameData)
        {}

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
