namespace FootBalLife.Database
{
    public class League
    {
        public int Id { get; internal set; }
        public string? Name { get; set; }
        public double CurrentRating { get; set; }
        public int ExtId { get; set; }

        internal int CountryId { get; set; }
        public Country? Country { get; internal set; }

        //public ICollection<Team> Teams { get; internal set; } = new List<Team>();
    }
}