namespace FootBalLife.GameDB.Models;

internal class League
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long? CurrentRating { get; set; }

    public long? CountryId { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<Team> Teams { get; } = new List<Team>();
}
