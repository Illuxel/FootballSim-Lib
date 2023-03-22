using BusinessLogicLayer.Services;
using DatabaseLayer.Repositories;
using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartupProject
{
    internal class MatchGeneratingExampleUsing
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

        public void GenerateMatch()
        {
            var teamRepository = new TeamRepository();
            var teams = teamRepository.Retrive(1);
            var homeTeam = teams[0];
            var guestTeam = teams[1];
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
