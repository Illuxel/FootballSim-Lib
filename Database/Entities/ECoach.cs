namespace FootBalLife.GameDB.Entities;

public class ECoach
{
    public string PersonId { get; set; } = null!;

    public virtual EPerson Person { get; internal set; } = null!;
}
