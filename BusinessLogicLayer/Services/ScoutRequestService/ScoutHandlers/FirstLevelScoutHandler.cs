using DatabaseLayer.Repositories;
using DatabaseLayer.Services;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services.ScoutRequestService
{
    internal class FirstLevelScoutHandler
    {
        protected PlayerGameData _playerGameData;
        protected LeagueRepository _leagueRepository;
        protected JuniorFinder _juniorFinder;
        protected int maxLeagueId = 10;

        public FirstLevelScoutHandler(PlayerGameData playerGameData)
        {
            _playerGameData = playerGameData;
            _leagueRepository = new LeagueRepository();
            _juniorFinder = new JuniorFinder();
        }
        public virtual int LookForAverageRatingInJuniorAcademy(string teamId)
        {
            var leagueId = defineLeagueId(_playerGameData.ClubId);
            var leagueIds = defineValidLeagueIds(leagueId);

            var isTeamInSameLeague = _leagueRepository.IsTeamInLeague(teamId,leagueIds);
            if(isTeamInSameLeague)
            {
                return _juniorFinder.AverageJuniorRatingByTeam(_playerGameData.ClubId);
            }
            return 0;
        }
        protected int defineLeagueId(string clubId)
        {
            return _leagueRepository.RetrieveLeagueIdByTeamId(clubId);
        }
        protected virtual List<int> defineValidLeagueIds(int leagueId)
        {
            return new List<int> {leagueId};
        }
    }
}
