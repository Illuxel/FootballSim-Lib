namespace FootBalLife.Database.Entities
{
    internal class ELeague
    {
        public long ID { get; internal set; }
        public string? Name { get; set; }
        public long CurrentRating { get; set; }

        public long CountryID { get; set; }
        public virtual ECountry? Country { get; internal set; }

        public virtual List<ETeam> Teams { get; internal set; } = new List<ETeam>();
    }
}