namespace FootBalLife.Database.Models
{
    public class League
    {
        public long ID { get; set; }
        public string? Name { get; set; }
        public long CurrentRating { get; set; }

        public long CountryID { get; set; }
        public virtual Country? Country { get; set; }

        public virtual ICollection<Team> Teams { get; } = new List<Team>();
    }
}