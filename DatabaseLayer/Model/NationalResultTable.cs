namespace DatabaseLayer
{
    public class NationalResultTable
    {
        public string Season { get; set; }

        public string TeamID { get; set; }
        public virtual Team Team { get; internal set; }

        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Loses { get; set; }

        public int ScoredGoals { get; set; }
        public int MissedGoals { get; set; }

        public int TotalPosition { get; set; }
    }
}