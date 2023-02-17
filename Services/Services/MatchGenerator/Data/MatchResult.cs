using FootBalLife.Database;

namespace FootBalLife.Services.MatchGenerator
{
    public class MatchResult
    {
        public Guid MatchId;

        public int HomeTeam;
        public int GuestTeam;

        public Team? Winner;

        public IEnumerable<Events.IMatchEvent> MatchHistory { get; set; }
        public IEnumerable<Guid> InjuredPlayers { get; set; }
        public IEnumerable<Guid> YellowCardPlayers { get; set; }
        public IEnumerable<Guid> RedCardPlayers { get; set; }
        public IEnumerable<Guid> ScoredPlayers { get; set; }
        public IEnumerable<Guid> AssistedPlayers { get; set; }
        public Events.IMatchEvent LastEvent { get; set; }
    }
}
