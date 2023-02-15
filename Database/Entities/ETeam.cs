namespace FootBalLife.GameDB.Entities;

public class ETeam
{
    public string Id { get; internal set; } = null!;

    public string Name { get; set; } = null!;

    public long? LeagueId { get; set; }

    public long? SportsDirectorId { get; set; }

    public long? CoachId { get; set; }
    public long? AgentId { get; set; }

    public long? IsNationalTeam { get; set; }

    public long? Strategy { get; set; }

    public string BaseColor { get; set; } = null!;

    public virtual ICollection<EContract> Contracts { get; internal set; } = new List<EContract>();

    public virtual ELeague? League { get; internal set; }

    public virtual ICollection<EMatch> MatchHomeTeamNavigations { get; internal set; } = new List<EMatch>();
    public virtual ICollection<EMatch> MatchGuestTeamNavigations { get; internal set; } = new List<EMatch>();

    public virtual ICollection<ENationalResultTable> NationalResultTables { get; internal set; } = new List<ENationalResultTable>();
}
