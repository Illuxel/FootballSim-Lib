using BusinessLogicLayer.Scenario;
using BusinessLogicLayer.Services;
using System;
using System.Diagnostics;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            var serv = new GenerateAllMatchesByTour(new DateTime(2023, 8, 12, 0, 0, 0));
            serv.Generate();

            var serv2 = new GenerateAllMatchesByTour(new DateTime(2023, 8, 19, 0, 0, 0));
            serv2.Generate();

            DateTime date = new DateTime(2023, 08, 20, 0, 0, 0);
            MatchTourDeterminer det = new MatchTourDeterminer();
            Console.WriteLine(det.DetermineTourNumber(1, date));
        }
    }
}
