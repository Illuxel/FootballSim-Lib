using System.Collections.Generic;

namespace DatabaseLayer
{
    public class Role
    {
        public int Id { get; internal set; }
        public string? Name { get; set; }
        public byte[]? Icon { get; set; }

        public int IsNpc { get; set; }

        public virtual ICollection<Person> People { get; internal set; } = new List<Person>();
    }
}