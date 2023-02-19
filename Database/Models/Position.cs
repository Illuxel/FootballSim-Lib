namespace FootBalLife.Database.Models
{
    public class Position
    {
        public long ID { get; set; }
        public long Location { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Player> Players { get; } = new List<Player>();
    }
}