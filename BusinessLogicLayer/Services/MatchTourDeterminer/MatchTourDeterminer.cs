using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class MatchTourDeterminer
    {
        MatchRepository _matchRepository;

        public MatchTourDeterminer()
        {
            _matchRepository = new MatchRepository();
        }
        public int DetermineTourNumber(int leagueId, DateTime gameDate)
        {
            var tourNumber = _matchRepository.Retrieve(gameDate,leagueId);
            return tourNumber;
        }
    }
}
