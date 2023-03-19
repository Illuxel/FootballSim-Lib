using Services.Services;
using System;

namespace StartupProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var playerFill = new FootballPlayerInfoGetter();
            var seasonCreator = new SeasonValueCreator();
            var currentSeason = seasonCreator.GetSeason(DateTime.Now.Year);
            playerFill.FillPlayerInfo(currentSeason);
        }
    }
}
