namespace FootBalLife.GameDB.Entities;

public class EPlayer
{
    public string PersonId { get; set; } = null!;
    public long? PositionId { get; set; }
    public string? ContractId { get; set; }
    
    public long Speed { get; set; }
    public long KickCount { get; set; }
    public long Endurance { get; set; }
    public long? Reflex { get; set; }
    public long Physics { get; set; }
    public long? Position { get; set; }
    public long Technique { get; set; }
    public long Passing { get; set; }

    public virtual EContract? Contract { get; internal set; }
    public virtual EPerson Person { get; internal set; } = null!;
    public virtual EPosition? PositionNavigation { get; internal set; }
}
