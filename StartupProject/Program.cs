using BusinessLogicLayer.Scenario;
using BusinessLogicLayer.Services;
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
            /*var tourStart = new DateTime(2023, 08, 12);
            for (int i = 0; i < 38; i++)
            {
                var tourGen = new GenerateGameActionsToNextMatch(tourStart);
                tourGen.SimulateActions();
                tourStart = tourStart.AddDays(7);
            }*/
            GoalRepository goalRep = new GoalRepository();
            var res = goalRep.GetTopGoalScorers("1", "2023/2024", "2023-08-12", "2024-05-19");
            Console.WriteLine();
        }
    }
}
