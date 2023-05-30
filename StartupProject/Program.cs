﻿using BusinessLogicLayer.Scenario;
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
            var serv = new GenerateAllMatchesByTour(new DateTime(2023, 8, 12, 0, 0, 0));
            serv.Generate();


            var serv2 = new GenerateAllMatchesByTour(new DateTime(2023, 8, 19, 0, 0, 0));
            serv2.Generate();


            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds.ToString());
        }
    }
}
