namespace FootBalLife.Database.Entities
{
    internal class ETeam
    {
        public string? ID { get; internal set; }
        public string? Name { get; set; }
        public string? BaseColor { get; set; }
        public StrategyType Strategy { get; set; }
        public long IsNationalTeam { get; set; }

        public long AgentID { get; set; }
        public long CoachID { get; set; }
        public long LeagueID { get; set; }
        public long SportsDirectorID { get; set; }

        public virtual ELeague? League { get; internal set; }

        public virtual ICollection<EContract> Contracts { get; internal set; } = new List<EContract>();

        public virtual ICollection<EMatch> MatchHomeTeamNavigations { get; internal set; } = new List<EMatch>();
        public virtual ICollection<EMatch> MatchGuestTeamNavigations { get; internal set; } = new List<EMatch>();

        public virtual ICollection<ENationalResultTable> NationalResultTables { get; internal set; } = new List<ENationalResultTable>();
    }
}