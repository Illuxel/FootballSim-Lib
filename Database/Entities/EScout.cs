namespace FootBalLife.GameDB.Entities;

public class EScout
{
    public string PersonId { get; internal set; } = null!;

    public virtual EPerson Person { get; internal set; } = null!;
}
