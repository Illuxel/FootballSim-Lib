namespace FootBalLife.Database.Entities
{
    internal class EMatch
    {
        public string ID { get; internal set; } = null!;

        public string HomeTeam { get; set; } = null!;
        public string GuestTeam { get; set; } = null!;

        public string Season { get; set; } = null!;

        public long WeekNumber { get; set; }

        public long HomeTeamGoals { get; set; }
        public long GuestTeamGoals { get; set; }

        public virtual ETeam HomeTeamNavigation { get; internal set; } = null!;
        public virtual ETeam GuestTeamNavigation { get; internal set; } = null!;
    }
}