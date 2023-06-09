using DatabaseLayer.Repositories;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class PlayerStatisticGetter
    {
        ContractRepository _contractRepository;
        MatchRepository _matchRepository;
        PlayerInMatchRepository _playerInMatchRepository;

        public PlayerStatisticGetter()
        {
            _contractRepository = new ContractRepository();
            _matchRepository = new MatchRepository();
            _playerInMatchRepository = new PlayerInMatchRepository();
        }

        public PlayerInvolvement GetPlayedMatch(string playerId, string season)
        {

            var teamId = getTeamId(playerId);

            var allMatchesCount = getAllMatchesCount(teamId, season);

            var playerInMatchesCount = getPlayerInMatchesCount(playerId);

            return new PlayerInvolvement() 
            {
                TotalMatch = allMatchesCount,
                PlayedMatch = playerInMatchesCount
            };
        }

        private string getTeamId(string playerId) => _contractRepository.Retrieve(playerId).First().TeamId;
        private int getAllMatchesCount(string teamId, string season) => _matchRepository.Retrieve(teamId, season).Count;
        private int getPlayerInMatchesCount(string playerId) => _playerInMatchRepository.Retrieve(playerId).Count;
    }
}
