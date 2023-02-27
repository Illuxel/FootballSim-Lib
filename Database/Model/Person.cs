using System.Collections.Generic;

namespace FootBalLife.Database
{
    public class Person
    {
        public string? Id { get; internal set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Birthday { get; set; }
        public byte[]? Icon { get; set; }

        public long CountryID { get; set; }
        public Country? Country { get; internal set; }

        public long? CurrentRoleID { get; set; }
        public Role? CurrentRole { get; internal set; }

        public ICollection<Contract> Contracts { get; internal set; } = new List<Contract>();
        /*
        public Agent? Agent { get; internal set; }
        public Coach? Coach { get; internal set; }
        public Director? Director { get; internal set; }
        public Player? Player { get; internal set; }
        public Scout? Scout { get; internal set; }
        */
    }
}