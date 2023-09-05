using DatabaseLayer.Model;
using DatabaseLayer.Repositories;
using System;
using System.Linq;

namespace DatabaseLayer.Services
{
    public class JuniorFinder
    {
        TeamRepository _teamRepository;
        public JuniorFinder()
        {
            _teamRepository = new TeamRepository();
        }

        public Player WorstJuniorPlayerByTeam(string teamId, DateTime gameDate)
        {
            var players = _teamRepository.RetrieveJuniors(teamId);
            if (players.Count != 0)
            {
                return players.OrderBy(x => x.Rating).First();
            }
            return null;
        }

        public Player BestJuniorPlayerByTeam(string teamId)
        {
            var players = _teamRepository.RetrieveJuniors(teamId);
            if (players.Count != 0)
            {
                var bestPlayer = players.OrderByDescending(x => x.Rating).First();

                return bestPlayer;
            }
            return null;
        }

        public int AverageJuniorRatingByTeam(string teamId)
        {
            var players = _teamRepository.RetrieveJuniors(teamId);
            if (players.Count != 0)
            {
                return (int)players.Average(x => x.Rating);
            }
            return 0;
        }
    }
}
