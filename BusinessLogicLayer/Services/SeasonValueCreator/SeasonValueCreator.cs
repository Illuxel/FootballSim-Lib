﻿using DatabaseLayer.Repositories;
using System;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class SeasonValueCreator
    {
        TeamRepository _teamRepository;
        public SeasonValueCreator()
        {
            _teamRepository = new TeamRepository();
        }
        public int GetStartYear(string season)
        {
            if (string.IsNullOrEmpty(season) || !season.Contains('/'))
            {
                return 0;
            }
            return Convert.ToInt32(season.Split('/')[0]);
        }
        public int GetEndYear(string season)
        {
            if (string.IsNullOrEmpty(season) || !season.Contains('/'))
            {
                return 0;
            }
            return Convert.ToInt32(season.Split('/')[1]);
        }

        public string GetSeason(DateTime gameDate)
        {
            var startSeasonDateByYear = GetSeasonStartDate(gameDate.Year);

            var year = gameDate.Year;
            if (gameDate < startSeasonDateByYear)
            {
                year--;
            }
            return getSeason(year);
        }

        public string GetSeason(int year)
        {
            return getSeason(year);
        }

        private string getSeason(int year)
        {
            if (year == 0)
            {
                return string.Empty;
            }
            var nextYear = year + 1;
            var season = string.Format("{0}/{1}", year, nextYear);
            return season;
        }

        public string GetFutureSeason(string currentSeason, int addedYears)
        {
            var startedYear = GetStartYear(currentSeason);

            return getSeason(startedYear + addedYears);
        }



        public DateTime GetSeasonStartDate(int year)
        {
            DateTime firstTourDate = new DateTime(year, 8, 1);
            while (firstTourDate.DayOfWeek != DayOfWeek.Saturday)
            {
                firstTourDate = firstTourDate.AddDays(1);
            }
            firstTourDate = firstTourDate.AddDays(7);
            return firstTourDate;
        }
        public DateTime GetSeasonStartDate(string season)
        {
            return GetSeasonStartDate(GetStartYear(season));
        }
        public DateTime GetSeasonEndDate(string season)
        {
            return getSeasonEndDate(GetStartYear(season));
        }
        private DateTime getSeasonEndDate(int year)
        {
            DateTime firstTourDate = GetSeasonStartDate(year);
            int maxNumberOfTours = calculateMaxNumberOfTours();
            return firstTourDate.AddDays(maxNumberOfTours * 7);
        }
        
        private int calculateMaxNumberOfTours()
        {
            var teams = _teamRepository.Retrieve();
            int maxNumberOfTeams = teams.GroupBy(x => x.LeagueID).Max(g => g.Count());
            int maxNumberOfTours = (maxNumberOfTeams - 1) * 2;
            return maxNumberOfTours;
        }
    }
}
