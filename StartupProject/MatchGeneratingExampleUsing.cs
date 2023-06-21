using BusinessLogicLayer.Services;
using DatabaseLayer.Repositories;
using DatabaseLayer;
using System;
using System.Linq;
using System.Diagnostics;

namespace StartupProject
{
    internal class MatchGeneratingExampleUsing
    {
        public static void OnMatchGoal(Goal goal)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine($"{goal.MatchMinute} !!!GOAL!!! from {goal.TeamId} scored: {goal.PlayerId}");
            Console.ResetColor();
        }
        public static void OnTeamChanged()
        {
            Console.WriteLine("Team changed");
        }
        public static void OnEventHappend(IMatchGameEvent gameEvent)
        {
            Console.WriteLine($"{gameEvent.MatchMinute} name: {gameEvent.EventCode} team: {gameEvent.HomeTeam.Name}");
            if (gameEvent.InjuredPlayer != null)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{gameEvent.MatchMinute} Injure from {gameEvent.HomeTeam.Name} player: {gameEvent.InjuredPlayer}");
                Console.ResetColor();
            }
            if (gameEvent.YellowCardPlayer != null)
            {
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{gameEvent.MatchMinute} Yellow card for {gameEvent.HomeTeam.Name} player: {gameEvent.YellowCardPlayer}");
                Console.ResetColor();
            }
            if (gameEvent.RedCardPlayer != null)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine($"{gameEvent.MatchMinute} Red card for {gameEvent.HomeTeam.Name} player: {gameEvent.RedCardPlayer}");
                Console.ResetColor();
            }
        }
        public static void OnMatchPaused()
        {
            Console.WriteLine("Match paused");
        }

        public void GenerateMatch()
        {
            var teamRepository = new TeamRepository();
            var teams = teamRepository.Retrieve(1);
            var homeTeam = teams[0];
            var guestTeam = teams[1];
            Console.WriteLine($"\nTeam {homeTeam.Name} with: {homeTeam.Id}");
            Console.WriteLine($"Team {guestTeam.Name} with: {guestTeam.Id}\n");

            var teamCreator = new TeamForMatchCreator();
            var homeTeamForMatch = teamCreator.Create(homeTeam);
            var guestTeamForMatch = teamCreator.Create(guestTeam);

            Console.WriteLine("\n-----" + homeTeamForMatch.Name + "-----/n");
            foreach (var player in homeTeamForMatch.MainPlayers)
            {
                Console.WriteLine(player.Value.RealPosition + " : " + player.Value.CurrentPlayer.Person.Surname + "("+ player.Value.CurrentPlayer.CurrentPlayerRating + ")");
            }

            Console.WriteLine("\n-----" + guestTeamForMatch.Name + "-----/n");
            foreach (var player in guestTeamForMatch.MainPlayers)
            {
                Console.WriteLine(player.Value.RealPosition + " : " + player.Value.CurrentPlayer.Person.Surname + "(" + player.Value.CurrentPlayer.CurrentPlayerRating + ")");
            }

            Console.WriteLine();
            var match = new MatchGenerator(homeTeamForMatch, guestTeamForMatch);

            match.OnMatchGoal += OnMatchGoal;

            match.OnMatchPaused += OnMatchPaused;
            match.OnMatchTeamChanged += OnTeamChanged;
            match.OnMatchEventHappend += OnEventHappend;

            match.StartGenerating();
            var result = match.MatchData;

            var homeTeamGoals = result.Goals.Where(item => item.TeamId == homeTeamForMatch.Id).Count();

            var guestTeamGoals = result.Goals.Where(item => item.TeamId == guestTeamForMatch.Id).Count();

            //Console.WriteLine(result.Winner.ToString());
            Console.WriteLine(homeTeamForMatch.Name + " " + homeTeamGoals + ":" + guestTeamGoals + " " + guestTeamForMatch.Name);


            foreach (var goal in result.Goals)
            {
                var scoredPlayerName = string.Empty;
                var goalPlayer = homeTeamForMatch.AllPlayers.FirstOrDefault(item => item.PersonID == goal.PlayerId);
                if (goalPlayer == null)
                {
                    goalPlayer = guestTeamForMatch.AllPlayers.FirstOrDefault(item => item.PersonID == goal.PlayerId);

                }
                Console.WriteLine($"{goal.MatchMinute} Scored by: {goalPlayer.Person.Surname}");
            }
        }
    }
}
