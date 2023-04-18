using DatabaseLayer.Enums;

namespace DatabaseLayer
{
    public class TeamTacticPosition
    {
        public string TeamId { get; set; }
        public string PlayerId { get; set; }
        public int IndexPosition { get; set; }
        public int CurrentPlayerRating { get; set; }
        public PlayerPositionGroup  PlayerGroup { get; set; }
    }
}
