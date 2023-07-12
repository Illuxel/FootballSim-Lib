using System;
using DatabaseLayer.Enums;
using DatabaseLayer.Model;
using DatabaseLayer.Repositories;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var criteria = new FindPlayersCriteria();
            criteria.AgeFrom = 18;
            criteria.AgeTo = 25;
            criteria.PositionCode = "GK";
            criteria.RatingFrom = 80;
            criteria.RatingTo = 100;
            var request = new ManagerRequestOfPlayers();
            request.ManagerId = "1";
            request.TeamId = "1";
            request.CreatedDate = DateTime.Now;
            request.Status = ManagerRequestStatus.InProgress;
            request.Criteria = criteria;

            var rep = new ManagerRequestOfPlayersRepository();
            rep.Insert(request);
            var response = rep.Retrieve();
        }
    }
}
