using BusinessLogicLayer.Services;
using System;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            /*var tourStart = new DateTime(2023, 08, 12);
            for (int i = 0; i < 38; i++)
            {û
                var tourGen = new GenerateGameActionsToNextMatch(tourStart);
                tourGen.SimulateActions();
                tourStart = tourStart.AddDays(7);
            }*/
            var serv = new PlayerGoalAssistsStatsService();
            var asists= serv.GetTopAssists("2023/2024", "1", 10);
            var bombardiers = serv.GetTopGoalScorers("2023/2024", "1", 10);
            Console.WriteLine();
        }
    }
}
