namespace FootBalLife.Database.Entities
{
    internal class ERole
    {
        public long ID { get; internal set; }

        public long? IsNpc { get; set; }

        public byte[]? Icon { get; set; }

        public string Name { get; set; } = null!;

        public virtual List<EPerson> People { get; internal set; } = new List<EPerson>();
    }
}