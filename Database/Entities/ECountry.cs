namespace FootBalLife.Database.Entities
{
    internal class ECountry
    {
        public long ID { get; internal set; }
        public string? Name { get; set; }
        public byte[]? Icon { get; set; }

        public virtual List<ELeague> Leagues { get; internal set; } = new List<ELeague>();
        public virtual List<EPerson> People { get; internal set; } = new List<EPerson>();
    }
}