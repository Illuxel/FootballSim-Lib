using DatabaseLayer;
using DatabaseLayer.Repositories;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class JuniorFinder
    {
        PlayerRepository _playerRepository;

        public JuniorFinder()
        {
            _playerRepository = new PlayerRepository();
        }

        public Player? WorstJuniorPlayerByTeam(string teamId)
        {
            var players = _playerRepository.RetrieveJuniorsByTeam(teamId);
            if(players.Count != 0)
            {
                return players.OrderBy(x => x.Rating).First();
            }
            return null;
        }

        public Player? BestJuniorPlayerByTeam(string teamId)
        {
            var players = _playerRepository.RetrieveJuniorsByTeam(teamId);
            if (players.Count != 0)
            {
                return players.OrderByDescending(x => x.Rating).First();
            }
            return null;
        }

        public int AverageJuniorRatingByTeam(string teamId)
        {
            var players = _playerRepository.RetrieveJuniorsByTeam(teamId);
            if (players.Count != 0)
            {
                return (int)players.Average(x => x.Rating);
            }
            return 0;
        }
    }
}
