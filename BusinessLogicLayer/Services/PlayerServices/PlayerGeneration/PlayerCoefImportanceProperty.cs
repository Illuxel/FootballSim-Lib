using DatabaseLayer.Enums;

namespace BusinessLogicLayer.Services
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
            Position = position;
            StrikeCoef = strikeCoef;
            SpeedCoef = speedCoef;
            PhysicsCoef = physicsCoef;
            DefendingCoef = defendingCoef;
            PassingCoef = passingCoef;
            DribblingCoef = driblingCoef;
        }
        internal PlayerCoefImportanceProperty()
        {

        }
        internal PlayerCoefImportanceProperty(PlayerPosition position)
        {
            Position = position;
        }
    }
}
