namespace FootBalLife.GameDB.Models;

internal class Contract
{
    public string Id { get; set; } = null!;

    public string SeasonFrom { get; set; } = null!;

    public string SeasonTo { get; set; } = null!;

    public string TeamId { get; set; } = null!;

    public string PersonId { get; set; } = null!;

    public long Price { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<Player> Players { get; } = new List<Player>();

    public virtual Team Team { get; set; } = null!;
}
