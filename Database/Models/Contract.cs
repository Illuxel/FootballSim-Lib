namespace FootBalLife.Database.Models
{
    public class Contract
    {
        public string? ID { get; set; }

        public string? TeamID { get; set; }
        public string? PersonID { get; set; }

        public string? SeasonTo { get; set; }
        public string? SeasonFrom { get; set; }

        public long Price { get; set; }

        public virtual Person? Person { get; set; }
        public virtual ICollection<Player> Players { get; } = new List<Player>();

        public virtual Team? Team { get; set; }
    }
}