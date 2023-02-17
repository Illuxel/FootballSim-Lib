using FootBalLife.Database.Models;

namespace FootBalLife.Services.MatchGenerator.Events
{
    public interface IMatchEvent
    {
        string EventCode { get; set; }
        string EventDescription { get; set; }

        int Duration { get; set; }
        int MatchMinute { get; set; }

        EventLocation Location { get; set; }

        Dictionary<string, double> BaseEventsChances { get; set; }
        Dictionary<string, double> NextEventsChances { get; }

        Team HomeTeam { get; set; }
        Team GuestTeam { get; set; }

        Guid? InjuredPlayer { get; }
        Guid? YellowCardPlayer { get; }
        Guid? RedCardPlayer { get; }
        Guid? ScoredPlayer { get; }
        Guid? AssistedPlayer { get; }
    }
}