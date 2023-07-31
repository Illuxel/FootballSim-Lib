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
            /*var tourStart = new DateTime(2023, 08, 12);
            for (int i = 1; i <= 38; i++)
            {
                var gTour1 = new GenerateAllMatchesByTour(tourStart);
                gTour1.Generate();
                tourStart = tourStart.AddDays(7);
            }*/
           
            /*var rep = new MatchRepository();
            var matches = rep.Retrieve("678065FDDB06C590A0D0F9EDC2B5196F");
            int w = 0, l = 0, d = 0;
            foreach (var match in matches)
            {
                if(match.GuestTeamId == "678065FDDB06C590A0D0F9EDC2B5196F")
                {
                    if(match.HomeTeamGoals > match.GuestTeamGoals)
                    {
                        l++;
                    }
                    else if(match.HomeTeamGoals < match.GuestTeamGoals)
                    {
                        w++;
                    }
                    else
                    {
                        d++;
                    }
                }
                else
                {
                    if (match.HomeTeamGoals > match.GuestTeamGoals)
                    {
                        w++;
                    }
                    else if (match.HomeTeamGoals < match.GuestTeamGoals)
                    {
                        l++;
                    }
                    else
                    {
                        d++;
                    }
                }
            }
            Console.WriteLine($"Wins : {w}\nLoses: {l}\n Draws: {d}");*/
            
            /*var res = new NationalResTabRepository().Retrieve();
            var bug = new List<NationalResultTable>();
            foreach (var first in res)
            {
                foreach(var sec in res)
                {
                    if(first.TeamID != sec.TeamID)
                    {
                        if(first.Wins == sec.Wins && first.Loses == sec.Loses && first.Draws == sec.Draws && bug.Contains(sec) == false)
                        {
                            bug.Add(sec);
                        }
                    }
                }
            }
            Console.WriteLine(bug.Count);*/
        }
    }
}
