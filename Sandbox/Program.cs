using System.Drawing;

using FootBalLife.Database;
using FootBalLife.Database.Repositories;

using FootBalLife.Services.MatchGenerator;
using FootBalLife.Services.MatchGenerator.Events;

internal class Program
{
    private static void Main(string[] args)
    {
        //TeamRepository teamRepository = new TeamRepository();

        //var list = teamRepository.();
        //Console.WriteLine(list.Count);

        //var playersStats = new GeneratePlayersStats(11, 35.0f, 4.5f);

        //playersStats.GenerateStats();

        //var team1 = new Team()
        //{
        //    ID = Guid.NewGuid(),
        //    Name = "Barselona",
        //    StrategyID = StrategyType.TotalPressing,
        //    Players = playersStats.GetPlayers(),
        //    TeamColor = Color.Red
        //};

        //playersStats.GenerateStats();

        //var team2 = new Team()
        //{
        //    ID = Guid.NewGuid(),
        //    Name = "Dinamo",
        //    StrategyID = StrategyType.BallСontrol,
        //    Players = playersStats.GetPlayers(),
        //    TeamColor = Color.Green
        //};

        //var match = new MatchFactory(team1, team2);

        //match.StartMatchGenerating();
        //var result = match.GetResult();

    }
}