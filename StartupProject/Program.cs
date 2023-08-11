using BusinessLogicLayer.Scenario;
using BusinessLogicLayer.Services;
using System;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var tourStart = new DateTime(2023, 08, 12);
            for (int i = 1; i <= 38; i++)
            {
                var tourGen = new GenerateGameActionsToNextMatch(tourStart);
                tourGen.SimulateActions();
                tourStart = tourStart.AddDays(7);
            }

            RatingActualizer ratingActualizer = new RatingActualizer(); 
            ratingActualizer.Actualize(tourStart);
        }
    }
}
