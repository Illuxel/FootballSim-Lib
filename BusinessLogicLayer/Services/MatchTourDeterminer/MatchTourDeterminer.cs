using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class MatchTourDeterminer
    {
        int _leagueId;
        DateTime _gameDate;
        MatchRepository _matchRepository;

        public MatchTourDeterminer(int leagueId, DateTime gameDate)
        {
            _leagueId = leagueId;
            _gameDate = gameDate;
            _matchRepository = new MatchRepository();
        }

        private List<Match> GetAllMatchesByLeague(int leagueId)
        {
            return _matchRepository.Retrieve(leagueId);
        }

        int GetLastTourNumber(List<Match> matches)
        {
            return matches[^1].TourNumber; 
        }

        private List<DateTime> GetSortedDates(List<Match> matches)
        {
            var dates = new HashSet<DateTime>();

            foreach (var match in matches)
            {
                if (DateTime.TryParse(match.MatchDate, out DateTime date))
                {
                    dates.Add(date.Date);
                }
            }

            var sortedDates = dates.OrderBy(x => x).ToList();

            return sortedDates;
        }

        private DateTime GetNextDate(DateTime date, List<DateTime> dates)
        {
            foreach (var dateTime in dates)
            {
                if (dateTime > date)
                {
                    return dateTime;
                }
            }

            return default;
        }

        private int GetCurrentTour(List<Match> matches, DateTime matchDate)
        {
            foreach (var match in matches)
            {
                if (DateTime.TryParse(match.MatchDate, out DateTime date) && date.Date == matchDate.Date)
                {
                    return match.TourNumber;
                }
            }
            return -1;
        }

        public int DetermineTourNumber()
        {
            var matches = GetAllMatchesByLeague(_leagueId);
            var lastNumber = GetLastTourNumber(matches);
            var sortedDates = GetSortedDates(matches);
            var closestDate = GetNextDate(_gameDate.Date, sortedDates);
            var tourNumber = GetCurrentTour(matches, closestDate);
            return tourNumber;
        }
    }
}
