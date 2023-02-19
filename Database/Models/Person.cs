namespace FootBalLife.Database.Models
{
    public class Person
    {
        public string? ID { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Birthday { get; set; }
        public byte[]? Icon { get; set; }

        public long CountryID { get; set; }
        public virtual Country? Country { get; set; }

        public long? CurrentRoleID { get; set; }
        public virtual Role? CurrentRole { get; set; }

        public virtual ICollection<Contract> Contracts { get; } = new List<Contract>();

        public virtual Agent? Agent { get; set; }
        public virtual Coach? Coach { get; set; }
        public virtual Director? Director { get; set; }
        public virtual Player? Player { get; set; }
        public virtual Scout? Scout { get; set; }
    }
}