namespace FootBalLife.GameDB.Entities;

public class EPerson
{
    public string Id { get; internal set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Birthday { get; set; } = null!;
    public long? CurrentRoleId { get; set; }
    public long CountryId { get; set; }
    public byte[]? Icon { get; set; }
    public virtual EAgent? Agent { get; internal set; }
    public virtual ECoach? Coach { get; internal set; }
    public virtual List<EContract> Contracts { get; internal set; } = new List<EContract>();
    public virtual ECountry Country { get; internal set; } = null!;
    public virtual ERole? CurrentRole { get; internal set; }
    public virtual EDirector? Director { get; internal set; }
    public virtual EPlayer? Player { get; internal set; }
    public virtual EScout? Scout { get; internal set; }
}
