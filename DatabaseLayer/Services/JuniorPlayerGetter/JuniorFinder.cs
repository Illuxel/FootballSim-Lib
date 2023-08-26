using DatabaseLayer.Model;
using DatabaseLayer.Repositories;
using System;
using System.Linq;

namespace DatabaseLayer.Services
{
    internal class JuniorFinder
    {
        TeamRepository _teamRepository;
        PersonRepository _personRepository;
        public JuniorFinder()
        {
            _teamRepository = new TeamRepository();
            _personRepository = new PersonRepository();
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

        public JuniorPlayerPreview BestJuniorPlayerByTeam(string teamId, DateTime gameDate)
        {
            var players = _teamRepository.RetrieveJuniors(teamId);
            if (players.Count != 0)
            {
                var bestPlayer = players.OrderByDescending(x => x.Rating).First();
                var person = _personRepository.Retrieve(bestPlayer.PersonID);
                var juniorPlayerPreview = new JuniorPlayerPreview()
                {
                    Rating = bestPlayer.Rating,
                    Position = bestPlayer.PositionCode,
                    NameSurname = person.Name != null ? $"{person.Name} {person.Surname}" : person.Surname,
                    Age = (int)(gameDate - person.Birthday).TotalDays / 365
                };
                return juniorPlayerPreview;
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
