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


        public RatingActualizer()
        {
            _teamRepository = new TeamRepository();
            _teamRatingCoeff = new TeamRatingWinCoeffRepository();
            _seasonValueCreator = new SeasonValueCreator();
        }
        public void Actualize(DateTime gameDate)
        {
            actualizeRating(averageCoeff(gameDate));
        }




        private Dictionary<string, double> averageCoeff(DateTime gameDate)
        {
            var lastSeasons = new List<string>();

            for(int i = gameDate.Year;i > gameDate.Year - 5;i--)
            {
                lastSeasons.Add(_seasonValueCreator.GetSeason(i));
            }


            var teamsRatingDict = new Dictionary<string, double>();

            var teamsStatsDict = _teamRatingCoeff.Retrieve(lastSeasons);

            
            foreach (var stat in teamsStatsDict)
            {

                teamsRatingDict.Add(stat.Key, stat.Value.Average(x => x.WinCoeff));
            }

            return teamsRatingDict; 
        }

        private void actualizeRating(Dictionary<string, double> teamsRatingDict)
        {
            var actualRating = new List<Team>();
            var list = teamsRatingDict.OrderByDescending(x => x.Value).ToList();

            foreach (var item in list)
            {
                var rating = list.IndexOf(item) + 1;

                if (rating != 0)
                {
                    var team = _teamRepository.Retrieve(item.Key);
                    team.CurrentInterlRatingPosition = rating;

                    actualRating.Add(team);
                }
            }
            _teamRepository.UpdateRating(actualRating);
        }
    }
}
