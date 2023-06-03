using DatabaseLayer.Repositories;
using System;

namespace BusinessLogicLayer.Services
{
    public class MatchTourDeterminer
    {
        MatchRepository _matchRepository;

        public MatchTourDeterminer()
        {
            _matchRepository = new MatchRepository();
        }
        public int GetTourNumber(int leagueId, DateTime gameDate)
        {
            var tourNumber = _matchRepository.GetTourNumber(gameDate,leagueId);
            return tourNumber;
        }
    }
}
