namespace FootBalLife.Database.Entities
{
    internal class EPlayer
    {
        public string? PersonID { get; set; }
        public long PositionID { get; set; }
        public string? ContractID { get; set; }
    
        public long KickCount { get; set; }
        public PlayerPostion Position { get; set; }

        public long Speed { get; set; }
        public long Passing { get; set; }
        public long Endurance { get; set; }
        public long Strike { get; set; }
        public long Physics { get; set; }
        public long Technique { get; set; }

        public virtual EPerson? Person { get; internal set; }

        public virtual EContract? Contract { get; internal set; }
        public virtual EPosition? PositionNavigation { get; internal set; }
    }
}