namespace FootBalLife.GameDB.Entities;

public class ELeague
{
    public long Id { get; internal set; }

    public string Name { get; set; } = null!;

    public long? CurrentRating { get; set; }

    public long? CountryId { get; set; }

    public virtual ECountry? Country { get; internal set; }

    public virtual List<ETeam> Teams { get; internal set; } = new List<ETeam>();
}
