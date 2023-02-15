namespace FootBalLife.GameDB.Entities;

public class ENationalResultTable
{
    public string Season { get; set; } = null!;

    public string TeamId { get; set; } = null!;

    public long Wins { get; set; }

    public long Draws { get; set; }

    public long Loses { get; set; }

    public long ScoredGoals { get; set; }

    public long MissedGoals { get; set; }

    public long TotalPosition { get; set; }

    public virtual ETeam Team { get; internal set; } = null!;
}
