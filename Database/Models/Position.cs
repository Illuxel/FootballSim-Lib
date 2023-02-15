namespace FootBalLife.GameDB.Models;

internal class Position
{
    public long Id { get; set; }

    public long Location { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Player> Players { get; } = new List<Player>();
}
