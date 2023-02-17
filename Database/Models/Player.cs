namespace FootBalLife.Database.Models
{
    public class Player
    {
        public string? PersonID { get; set; }

        public long? PositionID { get; set; }
        public PlayerPostion Position { get; set; }

        public string? ContractID { get; set; }
        public virtual Contract? Contract { get; set; }

        public long KickCount { get; set; }

        public long Speed { get; set; }
        public long Endurance { get; set; }
        public long Strike { get; set; }
        public long Physics { get; set; }
        public long Technique { get; set; }
        public long Passing { get; set; }

        public virtual Person? Person { get; set; }
        public virtual Position? PositionNavigation { get; set; }
    }
}