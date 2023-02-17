namespace FootBalLife.Database.Models
{
    public class Role
    {
        public long ID { get; set; }
        public string? Name { get; set; }
        public byte[]? Icon { get; set; }

        public long? IsNpc { get; set; }

        public virtual ICollection<Person> People { get; } = new List<Person>();
    }
}