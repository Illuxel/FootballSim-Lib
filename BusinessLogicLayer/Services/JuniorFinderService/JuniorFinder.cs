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

        public Player WorstJuniorPlayerByTeam(string teamId)
        {
            var players = _playerRepository.RetrieveJuniorsByTeam(teamId);
            return players.OrderBy(x => x.Rating).First();            
        }
        
        public Player BestJuniorPlayerByTeam(string teamId)
        {
            var players = _playerRepository.RetrieveJuniorsByTeam(teamId);
            return players.OrderByDescending(x => x.Rating).First();
        }
        
        public int AverageJuniorRatingByTeam(string teamId)
        {
            var players = _playerRepository.RetrieveJuniorsByTeam(teamId);
            return (int)players.Average(x => x.Rating);
        }
    }
}
