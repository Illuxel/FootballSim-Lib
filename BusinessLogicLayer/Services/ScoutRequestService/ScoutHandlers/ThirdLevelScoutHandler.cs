using DatabaseLayer.Services;

namespace BusinessLogicLayer.Services.ScoutRequestService
{
    internal class ThirdLevelScoutHandler : SecondLevelScoutHandler
    {
        public ThirdLevelScoutHandler(PlayerGameData playerGameData) : base(playerGameData)
        {}

        public virtual int LookForBestPlayerInJuniorAcademy(string teamId)
        {
            if (isCountOfRequestsPerWeekIsNotExceeded())
            {
                var leagueId = defineLeagueId(teamId);
                var leagueIds = defineValidLeagueIds(leagueId);

                var IsTeamInLeague = _leagueRepository.IsTeamInLeague(teamId, leagueIds);
                if (IsTeamInLeague)
                {
                    if (_playerGameData.CountOfRequestsPerTime == 0)
                    {
                        _playerGameData.CountOfRequestsPerTime = 1;
                    }
                    else
                    {
                        _playerGameData.CountOfRequestsPerTime++;
                    }
                    return _juniorFinder.BestJuniorPlayerByTeam(teamId).Rating;
                }
            }
                
            return 0;
        }

        protected override int _countOfRequestsPerTime
        {
            get => 3;
        }
    }
}
