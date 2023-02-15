namespace FootBalLife.GameDB.Entities;

public class EContract
{
    public string Id { get; internal set; } = null!;

    public string SeasonFrom { get; set; } = null!;

    public string SeasonTo { get; set; } = null!;

    public string TeamId { get; set; } = null!;

    public string PersonId { get; set; } = null!;

    public long Price { get; set; }

    public virtual EPerson Person { get; internal set; } = null!;

    public virtual List<EPlayer> Players { get; internal set; } = new List<EPlayer>();

    public virtual ETeam Team { get; internal set; } = null!;
}
