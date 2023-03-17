namespace FootBalLife.Database
{
    public class Goal
    {
        public string Id { get; set; }
        public string MatchId { get; set; }
        public string PlayerId { get; set; }
        public string TeamId { get; set; }
        public int MatchMinute { get; set; }
        public string AssistPlayerId { get; set; }
    }
}
