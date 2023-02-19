namespace FootBalLife.Database.Entities
{
    internal class EPerson
    {
        public string ID { get; internal set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Birthday { get; set; }
        public long? CurrentRoleID { get; set; }
        public long CountryID { get; set; }
        public byte[]? Icon { get; set; }
        public virtual ECountry Country { get; internal set; }
        public virtual List<EContract> Contracts { get; internal set; }

        public virtual ERole? CurrentRole { get; internal set; }

        public virtual EAgent? Agent { get; internal set; }
        public virtual ECoach? Coach { get; internal set; }
        public virtual EDirector? Director { get; internal set; }
        public virtual EPlayer? Player { get; internal set; }
        public virtual EScout? Scout { get; internal set; }
    }
}