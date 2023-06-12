using DatabaseLayer.Repositories;
using System;
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

        private string getTeamId(string playerId)
        {
            if (string.IsNullOrEmpty(playerId))
            {
                throw new Exception("Player ID is not set");
            }
            else
            {
                return _contractRepository.Retrieve(playerId).First().TeamId;
            }
        }
        private int getAllMatchesCount(string teamId, string season)
        {
            if (string.IsNullOrEmpty(teamId) || string.IsNullOrEmpty(season))
            {
                throw new Exception("Incoming value is not set");
            }
            return _matchRepository.Retrieve(teamId, season).Count;
        }
        private int getPlayerInMatchesCount(string playerId)
        {
            if (string.IsNullOrEmpty(playerId))
            {
                throw new Exception("Player ID is not set");
            }

            return _playerInMatchRepository.Retrieve(playerId).Count;
        }
    }
}
