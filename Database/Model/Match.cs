namespace FootBalLife.Database
{
    public class Match
    {
        public string? ID { get; internal set; }

        public string? Season { get; set; }
        public long WeekNumber { get; set; }

        public string? HomeTeam { get; set; }
        public string? GuestTeam { get; set; }

        public long HomeTeamGoals { get; set; }
        public long GuestTeamGoals { get; set; }

        public virtual Team? HomeTeamNavigation { get; internal set; }
        public virtual Team? GuestTeamNavigation { get; internal set; }
    }
}