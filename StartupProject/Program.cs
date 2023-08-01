using BusinessLogicLayer.Scenario;
using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var tourStart = new DateTime(2023, 08, 12);
            for (int i = 1; i <= 38; i++)
            {
                var gTour1 = new GenerateAllMatchesByTour(tourStart);
                gTour1.Generate();
                tourStart = tourStart.AddDays(7);
            }
        }
    }
}
