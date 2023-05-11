using DatabaseLayer;
using DatabaseLayer.DBSettings;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace BusinessLogicLayer.Services
{
    public class RatingActualizer
    {

        private Dictionary<string, double> _averageCoeff = new Dictionary<string, double>();
        private void fillCoeffDictionary()
        {
            var team = new TeamRepository();
            var winCoeff = new TeamRatingWinCoeffRepository();

            var teams = winCoeff.RetrieveAllTeams();

            foreach(var t in teams)
            {
                var response = winCoeff.RetrieveAllSeasonsByTeam(t);
                if (response != null)
                {
                    Console.WriteLine(string.Join(" ",response.Select(x=>x.WinCoeff)));
                    _averageCoeff.Add(t, response.Average(x => x.WinCoeff));
                }
            }
            
        }
        private void actualizeRating()
        {
            var team = new TeamRepository();
            var list = _averageCoeff.OrderByDescending(x=>x.Value).ToList();
            foreach ( var item in list)
            {
                var rating = list.IndexOf(item) + 1;
                if(rating != 0)
                {
                    team.UpdateRating(item.Key, rating);
                }
            }
        }
        public void Actualize()
        {
            fillCoeffDictionary();
            actualizeRating();
        }
    }
}
