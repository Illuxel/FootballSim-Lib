namespace FootBalLife.GameDB.Models;

internal class Player
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

    public virtual Contract? Contract { get; set; }
    public virtual Person Person { get; set; } = null!;
    public virtual Position? PositionNavigation { get; set; }
}
