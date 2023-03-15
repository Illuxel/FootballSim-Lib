using System.Collections.Generic;

namespace FootBalLife.Database
{
    public class Country
    {
        public int Id { get; internal set; }

        public string? Name { get; set; }
        public byte[]? Icon { get; set; }
        public ICollection<League> Leagues { get; internal set; } = new List<League>();
    }
}