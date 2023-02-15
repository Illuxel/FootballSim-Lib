namespace FootBalLife.GameDB.Entities;

public class ERole
{
    public long Id { get; internal set; }

    public long? IsNpc { get; set; }

    public byte[]? Icon { get; set; }

    public string Name { get; set; } = null!;

    public virtual List<EPerson> People { get; internal set; } = new List<EPerson>();
}
