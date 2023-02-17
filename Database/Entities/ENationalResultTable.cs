namespace FootBalLife.Database.Entities
{
    internal class ENationalResultTable
    {
        public string? Season { get; set; }

        public string? TeamID { get; set; }

        public long TotalPosition { get; set; }

        public long Wins { get; set; }
        public long Draws { get; set; }
        public long Loses { get; set; }

        public long ScoredGoals { get; set; }
        public long MissedGoals { get; set; }

        public virtual ETeam? Team { get; internal set; }
    }
}