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
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var serv = new GenerateAllMatchesByTour(new DateTime(2023, 8, 12, 0, 0, 0));
            serv.Generate();

            DateTime date = new DateTime(2023, 08, 17, 0, 0, 0);
            MatchTourDeterminer det = new MatchTourDeterminer(1,date);
            Console.WriteLine(det.DetermineTourNumber());
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
