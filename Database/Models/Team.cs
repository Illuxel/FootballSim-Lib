namespace FootBalLife.Database.Models
{
    public class Team
    {
        public string? ID { get; set; }
        public string? Name { get; set; }
        public string? BaseColor { get; set; }
        public StrategyType Strategy { get; set; }
        public long IsNationalTeam { get; set; }

        public long LeagueID { get; set; }

        public long AgentID { get; set; }
        public long CoachID { get; set; }
        public long SportsDirectorID { get; set; }

        public virtual League? League { get; set; }

        public virtual ICollection<Contract> Contracts { get; } = new List<Contract>();

        public virtual ICollection<Match> MatchHomeTeamNavigations { get; } = new List<Match>();
        public virtual ICollection<Match> MatchGuestTeamNavigations { get; } = new List<Match>();

        public virtual ICollection<NationalResultTable> NationalResultTables { get; } = new List<NationalResultTable>();

        // returns first player that have playerPostion
        public Player GetPlayer(PlayerPostion playerPostion)
        {
            Contracts.First().Players

            var selectedPlayers = Players.Where(player => player.PositionID == (long)playerPostion);
            return selectedPlayers.MaxBy(player => player.Endurance);
        }

        public double AvgSpeed(PlayerPostion playerPostion = PlayerPostion.All)
        {
            return playerPostion == PlayerPostion.All
                ? Players.Average(p => p.Speed)
                : Players
                    .Where(p => p.PositionID == (long)playerPostion)
                    .Average(p => p.Speed);
        }
        public double AvgStrike(PlayerPostion playerPostion = PlayerPostion.All)
        {
            return playerPostion == PlayerPostion.All
                ? Players.Average(p => p.Strike)
                : Players
                    .Where(p => p.PositionID == (long)playerPostion)
                    .Average(p => p.Strike);
        }
        public double AvgDefense(PlayerPostion playerPostion = PlayerPostion.All)
        {
            return playerPostion == PlayerPostion.All
                ? Players.Average(p => p.Endurance)
                : Players
                    .Where(p => p.PositionID == (long)playerPostion)
                    .Average(p => p.Endurance);
        }
        public double AvgPhysicalTraining(PlayerPostion playerPostion = PlayerPostion.All)
        {
            return playerPostion == PlayerPostion.All
                ? Players.Average(p => p.Physics)
                : Players
                    .Where(p => p.PositionID == (long)playerPostion)
                    .Average(p => p.Physics);
        }
        public double AvgTechnique(PlayerPostion playerPostion = PlayerPostion.All)
        {
            return playerPostion == PlayerPostion.All
                ? Players.Average(p => p.Technique)
                : Players
                    .Where(p => p.PositionID == (long)playerPostion)
                    .Average(p => p.Technique);
        }
        public double AvgPassing(PlayerPostion playerPostion = PlayerPostion.All)
        {
            return playerPostion == PlayerPostion.All
                ? Players.Average(p => p.Passing)
                : Players
                    .Where(p => p.PositionID == (long)playerPostion)
                    .Average(p => p.Passing);
        }
    }
}