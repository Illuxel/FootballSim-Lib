using System;
using System.Collections.Generic;

namespace DatabaseLayer
{
    public class Person
    {
        public string Id { get; internal set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthday { get; set; }
        public byte[] Icon { get; set; }

        public int CountryID { get; set; }
        public Country Country { get; internal set; }

        public int CurrentRoleID { get; set; }
        public Role CurrentRole { get; internal set; }

        public ICollection<Contract> Contracts { get; internal set; }
        public int GetAge(DateTime gameDate)
        {
            return gameDate.Year - Birthday.Year;
        }

        public Person()
        {
            Id = Guid.NewGuid().ToString();
        }

        /*
        public Agent? Agent { get; internal set; }
        public Coach? Coach { get; internal set; }
        public Director? Director { get; internal set; }
        public Player? Player { get; internal set; }
        public Scout? Scout { get; internal set; }
        */
    }
}