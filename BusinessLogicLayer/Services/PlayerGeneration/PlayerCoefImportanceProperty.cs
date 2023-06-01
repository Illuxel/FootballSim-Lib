using DatabaseLayer.Enums;

namespace BusinessLogicLayer.Services.PlayerGeneration
{
    internal class PlayerCoefImportanceProperty
    {
        public PlayerPosition Position { get; set; }
        public double StrikeCoef { get; set; }
        public double SpeedCoef { get; set; }
        public double PhysicsCoef { get; set; }
        public double DefendingCoef { get; set; }
        public double PassingCoef { get; set; }
        public double DribblingCoef { get; set; }
        internal PlayerCoefImportanceProperty(PlayerPosition position, double strikeCoef, double speedCoef, double physicsCoef, double defendingCoef, double passingCoef, double driblingCoef)
        {
            this.Position = position;
            this.StrikeCoef = strikeCoef;
            this.SpeedCoef = speedCoef;
            this.PhysicsCoef = physicsCoef;
            this.DefendingCoef = defendingCoef;
            this.PassingCoef = passingCoef;
            this.DribblingCoef= driblingCoef;    
        }
        internal PlayerCoefImportanceProperty()
        {

        }
        internal PlayerCoefImportanceProperty(PlayerPosition position)
        {
            this.Position = position;
        }
    }
}
