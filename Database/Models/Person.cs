namespace FootBalLife.GameDB.Models;

internal class Person
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Birthday { get; set; } = null!;
    public long? CurrentRoleId { get; set; }
    public long CountryId { get; set; }
    public byte[]? Icon { get; set; }
    public virtual Agent? Agent { get; set; }
    public virtual Coach? Coach { get; set; }
    public virtual ICollection<Contract> Contracts { get; } = new List<Contract>();
    public virtual Country Country { get; set; } = null!;
    public virtual Role? CurrentRole { get; set; }
    public virtual Director? Director { get; set; }
    public virtual Player? Player { get; set; }
    public virtual Scout? Scout { get; set; }
}
