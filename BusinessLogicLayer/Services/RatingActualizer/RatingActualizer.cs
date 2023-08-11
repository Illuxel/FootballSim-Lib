using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class RatingActualizer
    {
        TeamRepository _teamRepository;
        TeamRatingWinCoeffRepository _teamRatingCoeff;
        SeasonValueCreator _seasonValueCreator;
        NationalResTabRepository _nationalResTabRepository;
        private static int _countYearsForRating = 5;
        private static string? _currentSeason;

        public RatingActualizer()
        {
            _teamRepository = new TeamRepository();
            _teamRatingCoeff = new TeamRatingWinCoeffRepository();
            _seasonValueCreator = new SeasonValueCreator();
            _nationalResTabRepository = new NationalResTabRepository();
        }
        public void Actualize(DateTime gameDate)
        {
            _currentSeason = _seasonValueCreator.GetSeason(gameDate.Year - 1);
            var teamsWinCoef = getAverageCoeff(gameDate.Year);
            actualizeRating(teamsWinCoef);
        }

        private Dictionary<Team, double> getAverageCoeff(int currentYear)
        {
            var lastSeasons = new List<string>();
            for (int i = currentYear; i > currentYear - _countYearsForRating; i--)
            {
                lastSeasons.Add(_seasonValueCreator.GetSeason(i));
            }

            var teamsRatingDict = new Dictionary<Team, double>();
            var teamsStatsDict = _teamRatingCoeff.Retrieve(lastSeasons);

            var allTeams = _teamRepository.Retrieve();

            var coefs = new List<TeamRatingWinCoeff>();

            foreach (var stat in teamsStatsDict)
            {
                var winCoeffBySeason = stat.Value.FirstOrDefault(x => x.Season == _currentSeason);
                if (winCoeffBySeason != null && winCoeffBySeason.WinCoeff == 0)
                {
                    var resTable = _nationalResTabRepository.Retrieve(stat.Key, _currentSeason);

                    var currentSeasonResTable = resTable.FirstOrDefault(x => x.Season == _currentSeason);
                    if (currentSeasonResTable != null)
                    {
                        winCoeffBySeason.WinCoeff = (double)currentSeasonResTable.Wins / (currentSeasonResTable.Wins + currentSeasonResTable.Loses + currentSeasonResTable.Draws) * 100.0;
                        coefs.Add(winCoeffBySeason);
                    }
                }

                var team = allTeams.FirstOrDefault(t => t.Id == stat.Key);
                if (team != null)
                {
                    teamsRatingDict.Add(team, stat.Value.Average(x => x.WinCoeff));
                }
            }

            _teamRatingCoeff.Update(coefs);

            return teamsRatingDict;
        }


        private void actualizeRating(Dictionary<Team, double> teamsRatingDict)
        {
            var actualRating = new List<Team>();

            foreach(var team in teamsRatingDict)
            {
                team.Key.GlobalPoints = (int)(team.Value * team.Key.League.CurrentRating);
            }

            var list = teamsRatingDict.Keys.OrderByDescending(x=>x.GlobalPoints).ToList();
         
            foreach (var item in list)
            {
                var rating = list.IndexOf(item) + 1;

                if (rating != 0)
                {
                    item.CurrentInterlRatingPosition = rating;

                    actualRating.Add(item);
                }
            }
            _teamRepository.UpdateRating(actualRating);
        }
        
    }
}
