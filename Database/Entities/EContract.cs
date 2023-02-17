namespace FootBalLife.Database.Entities
{
    internal class EContract
    {
        public string? ID { get; internal set; }

        public string? SeasonTo { get; set; }
        public string? SeasonFrom { get; set; }

        public string? PersonID { get; set; }
        public virtual EPerson? Person { get; internal set; }

        public string? TeamID { get; set; }
        public virtual ETeam? Team { get; internal set; }

        public long Price { get; set; }

        public virtual List<EPlayer> Players { get; internal set; } = new List<EPlayer>();
    }
}