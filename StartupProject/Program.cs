using BusinessLogicLayer.Scenario;
using BusinessLogicLayer.Services;
using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Linq;

namespace StartupProject
{
    internal class Program
    {
       
        public static void Main(string[] args)
        {
            var serv = new GenerateAllMatchesByTour(DateTime.Now);
            serv.GenerateAllMatches();
        }
    }
}
