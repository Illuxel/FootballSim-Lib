namespace FootBalLife.Database.Models
{
    public class Country
    {
        public long ID { get; set; }

        public string? Name { get; set; }
        public byte[]? Icon { get; set; }

        public virtual ICollection<Person> People { get; } = new List<Person>();
        public virtual ICollection<League> Leagues { get; } = new List<League>();
    }
}