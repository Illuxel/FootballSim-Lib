using BusinessLogicLayer.Scenario;
using System;
using System.Diagnostics;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var serv = new GenerateAllMatchesByTour(new DateTime(2023, 12, 30, 0, 0, 0));
            serv.Generate();

            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds.ToString());
        }
    }
}
