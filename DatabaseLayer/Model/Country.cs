using System.Collections.Generic;

namespace DatabaseLayer
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public byte[] Icon { get; set; }
        public int ExtId { get; set; }

        public ICollection<League> Leagues { get; internal set; } = new List<League>();
    }


}