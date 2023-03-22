using DatabaseLayer;
using System;
using System.Drawing;
using BusinessLogicLayer;
using BusinessLogicLayer.Services.MatchGenerator;
using DatabaseLayer.Repositories;

namespace StartupProject
{
    internal class Program
    {
        public static void OnMatchGoal(Goal goal)
        {
            Console.WriteLine($"Goal from {goal.TeamId} current time: {goal.MatchMinute}");
        }
        public static void OnTeamChanged()
        {
            Console.WriteLine("Team changed");
        }
        public static void OnEventHappend(IMatchGameEvent gameEvent)
        {
            Console.WriteLine($"Event happened minute: {gameEvent.MatchMinute} name: {gameEvent.EventCode} team: {gameEvent.HomeTeam.Name}");
        }
        public static void OnMatchPaused()
        {
            Console.WriteLine("Match paused");
        }

        public static void Main(string[] args)
        {
            /* var playersStats = new PlayerStatsGen(25, 45);

             playersStats.Start();

             var homeTeam = new Team()
             {
                 Id = Guid.NewGuid(),
                 Name = "Barselona",
                 Strategy = StrategyType.TotalPressing, // initial strategy
                 Players = playersStats.GetPlayers(),
                 BaseColor = Color.Red.ToString(),
             };

             playersStats.Start();

             var guestTeam = new Team()
             {
                 Id = Guid.NewGuid(),
                 Name = "Dinamo",
                 Strategy = StrategyType.BallСontrol, // initial strategy
                 Players = playersStats.GetPlayers(),
                 BaseColor = Color.Red.ToString()
             };
            */
            var teamRepository = new TeamRepository();
            var teams = teamRepository.Retrive(1);
            var homeTeam = teams[0];// teamRepository.Retrive("015834FD9556AAEC44DE54CDE350235B");
            var guestTeam = teams[1]; //teamRepository.Retrive("B5551778D1672E4E544F32BFFAD52BA6");
            Console.WriteLine($"\nTeam {homeTeam.Name} with: {homeTeam.Id}");
            Console.WriteLine($"Team {guestTeam.Name} with: {guestTeam.Id}\n");

            var match = new MatchGenerator(homeTeam, guestTeam);

            match.OnMatchGoal += OnMatchGoal;

            match.OnMatchPaused += OnMatchPaused;
            match.OnMatchTeamChanged += OnTeamChanged;
            match.OnMatchEventHappend += OnEventHappend;

            match.StartGenerating();
            var result = match.MatchData;

            Console.WriteLine(result.Winner.ToString());
        }
    }
}
