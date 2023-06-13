using System;

namespace DatabaseLayer
{
    public class PlayerInMatch
    {
        public string Id { get; set; }
        public string MatchId { get; set; }
        public string PlayerId { get; set; }
        public string TeamId { get; set; }
        public int StartMinute { get; set; }
        public int LastMinute { get; set; }

        public PlayerInMatch()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
