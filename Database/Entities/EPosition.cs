namespace FootBalLife.GameDB.Entities;

public class EPosition
{
    public long Id { get; internal set; }

    public long Location { get; set; }

    public string Name { get; set; } = null!;

    public virtual List<EPlayer> Players { get; internal set; } = new List<EPlayer>();
}
