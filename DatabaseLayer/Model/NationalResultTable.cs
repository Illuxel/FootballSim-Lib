namespace DatabaseLayer
{
    public class NationalResultTable
    {
        public string Season { get; internal set; }

        public string TeamID { get; set; }
        public virtual Team Team { get; internal set; }

        public long Wins { get; set; }
        public long Draws { get; set; }
        public long Loses { get; set; }

        public long ScoredGoals { get; set; }
        public long MissedGoals { get; set; }

        public long TotalPosition { get; set; }
    }
}