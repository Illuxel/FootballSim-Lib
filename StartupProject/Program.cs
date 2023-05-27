using BusinessLogicLayer.Scenario;
using BusinessLogicLayer.Services;
using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Diagnostics;
using System.Linq;

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
