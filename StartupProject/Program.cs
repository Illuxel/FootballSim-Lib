using DatabaseLayer.Repositories;
using System;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            /////////ADD "SEASON" FIELD TO DB INTO "MATCH" TABLE
            /*var startDate = new DateTime(2023, 8, 19); 
            for (int i = 1; i < 38; i++)
            {
                var serv = new GenerateGameActionsToNextMatch(startDate);
                serv.SimulateActions();
                startDate = startDate.AddDays(7);
            }*/
            var value = new GoalRepository().GetTopAssistents("2023/2024", "1", 10);
            foreach (var item in value)
            {
                Console.WriteLine($"{item.Key} {item.Value.Assists} {item.Value.MatchesPlayed}");
            }
        }
    }
}
