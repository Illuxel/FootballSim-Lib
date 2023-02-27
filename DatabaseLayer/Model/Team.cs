﻿using System.Collections.Generic;
using System.Linq;

namespace FootBalLife.Database
{
    public class Team
    {
        public string? Id { get; internal set; }
        public string? Name { get; set; }
        public string? BaseColor { get; set; }
        public StrategyType Strategy { get; set; }
        public int IsNationalTeam { get; set; }

        public int LeagueID { get; set; }
        public League? League { get; internal set; }

        public string ScoutID { get; set; }
        public string CoachID { get; set; }
        public string SportsDirectorId { get; set; }
        /*
        public virtual ICollection<Contract> Contracts { get; internal set; } = new List<Contract>();

        
        public ICollection<Match> MatchHomeTeamNavigations { get; internal set; } = new List<Match>();
        public ICollection<Match> MatchGuestTeamNavigations { get; internal set; } = new List<Match>();
        
        public ICollection<NationalResultTable> NationalResultTables { get; internal set; } = new List<NationalResultTable>();
        
        // returns first player that have playerPostion
        public Player GetPlayer(PlayerPosition playerPostion)
        {
            //Contracts.First().Players

            var selectedPlayers = Contracts.Where(player => player.PositionID == (long)playerPostion);
            return selectedPlayers.MaxBy(player => player.Endurance);
        }
        public double AvgSpeed(PlayerPosition playerPostion = PlayerPosition.All)
        {
            return playerPostion == PlayerPosition.All
                ? Players.Average(p => p.Speed)
                : Players
                    .Where(p => p.PositionID == (long)playerPostion)
                    .Average(p => p.Speed);
        }
        public double AvgStrike(PlayerPosition playerPostion = PlayerPosition.All)
        {
            return playerPostion == PlayerPosition.All
                ? Players.Average(p => p.Strike)
                : Players
                    .Where(p => p.PositionID == (long)playerPostion)
                    .Average(p => p.Strike);
        }
        public double AvgDefense(PlayerPosition playerPostion = PlayerPosition.All)
        {
            return playerPostion == PlayerPosition.All
                ? Players.Average(p => p.Endurance)
                : Players
                    .Where(p => p.PositionID == (long)playerPostion)
                    .Average(p => p.Endurance);
        }
        public double AvgPhysicalTraining(PlayerPosition playerPostion = PlayerPosition.All)
        {
            return playerPostion == PlayerPosition.All
                ? Players.Average(p => p.Physics)
                : Players
                    .Where(p => p.PositionID == (long)playerPostion)
                    .Average(p => p.Physics);
        }
        public double AvgTechnique(PlayerPosition playerPostion = PlayerPosition.All)
        {
            return playerPostion == PlayerPosition.All
                ? Players.Average(p => p.Technique)
                : Players
                    .Where(p => p.PositionID == (long)playerPostion)
                    .Average(p => p.Technique);
        }
        public double AvgPassing(PlayerPosition playerPostion = PlayerPosition.All)
        {
            return playerPostion == PlayerPosition.All
                ? Players.Average(p => p.Passing)
                : Players
                    .Where(p => p.PositionID == (long)playerPostion)
                    .Average(p => p.Passing);
        }*/
    }
}