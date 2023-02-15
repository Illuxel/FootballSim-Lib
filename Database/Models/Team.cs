namespace FootBalLife.GameDB.Models;

internal class Team
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public long? LeagueId { get; set; }

    public long? SportsDirectorId { get; set; }

    public long? CoachId { get; set; }

    public long? AgentId { get; set; }

    public long? IsNationalTeam { get; set; }

    public long? Strategy { get; set; }

    public string BaseColor { get; set; } = null!;

    public virtual ICollection<Contract> Contracts { get; } = new List<Contract>();

    public virtual League? League { get; set; }

    public virtual ICollection<Match> MatchTeam1Navigations { get; } = new List<Match>();

    public virtual ICollection<Match> MatchTeam2Navigations { get; } = new List<Match>();

    public virtual ICollection<NationalResultTable> NationalResultTables { get; } = new List<NationalResultTable>();
}
