using DatabaseLayer;
using System;
using System.Drawing;
using BusinessLogicLayer;
using BusinessLogicLayer.Services;
using DatabaseLayer.Repositories;

namespace StartupProject
{
    internal class Program
    {
       
        public static void Main(string[] args)
        {
            var z = 1;
            var leagueR = new LeagueRepository();
            var leagues = leagueR.Retrive();

            //var te = 2;
            var teamsR = new TeamRepository();
            var teams = teamsR.Retrive(1);
            var t = 1;
        }
    }
}
