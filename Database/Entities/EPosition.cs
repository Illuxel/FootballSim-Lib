namespace FootBalLife.Database.Entities
{
    internal class EPosition
    {
        public long ID { get; internal set; }
        public long Location { get; set; }

        public string Name { get; set; } = null!;

        public virtual List<EPlayer> Players { get; internal set; } = new List<EPlayer>();
    }
}