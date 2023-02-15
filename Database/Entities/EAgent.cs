namespace FootBalLife.GameDB.Entities;

public class EAgent
{
    public string PersonId { get; set; } = null!;

    public virtual EPerson Person { get; internal set; } = null!;
}
