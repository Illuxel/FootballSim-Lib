namespace FootBalLife.GameDB.Entities;

public class ECountry
{
    public long Id { get; internal set; }

    public string Name { get; set; } = null!;

    public byte[]? Icon { get; set; }

    public virtual List<ELeague> Leagues { get; internal set; } = new List<ELeague>();

    public virtual List<EPerson> People { get; internal set; } = new List<EPerson>();
}
