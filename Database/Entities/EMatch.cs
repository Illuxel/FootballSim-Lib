namespace FootBalLife.GameDB.Entities;

public class EMatch
{
    public string Id { get; internal set;} = null!;

    public string HomeTeam { get; set; } = null!;
    public string GuestTeam { get; set; } = null!;

    public string Season { get; set; } = null!;

    public long WeekNumber { get; set; }

    public long HomeTeamGoals { get; set; }
    public long GuestTeamGoals { get; set; }

    public virtual ETeam HomeTeamNavigation { get; internal set; } = null!;
    public virtual ETeam GuestTeamNavigation { get; internal set; } = null!;
}
