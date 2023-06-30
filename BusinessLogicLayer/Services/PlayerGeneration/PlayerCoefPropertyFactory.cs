using DatabaseLayer.Enums;

namespace BusinessLogicLayer.Services
{
    internal class PlayerCoefPropertyFactory
    {
        internal PlayerCoefImportanceProperty Create(PlayerPosition position)
        {
            PlayerCoefImportanceProperty playerCoef = new PlayerCoefImportanceProperty();
            
            switch (position)
            {
                case PlayerPosition.Goalkeeper:
                    playerCoef.SpeedCoef = 0.5;
                    playerCoef.StrikeCoef = 0.95;
                    playerCoef.DefendingCoef = 1;
                    playerCoef.PassingCoef = 1;
                    playerCoef.DribblingCoef = 1;
                    playerCoef.PhysicsCoef = 1;
                    break;

                case PlayerPosition.RightDefender:
                case PlayerPosition.LeftDefender:
                    playerCoef.SpeedCoef = 1;
                    playerCoef.StrikeCoef = 0.85;
                    playerCoef.DefendingCoef = 1;
                    playerCoef.PassingCoef = 1;
                    playerCoef.DribblingCoef = 1;
                    playerCoef.PhysicsCoef = 0.9;
                    break;

                case PlayerPosition.CentralDefender:
                    playerCoef.SpeedCoef = 0.9;
                    playerCoef.StrikeCoef = 0.6;
                    playerCoef.DefendingCoef = 1;
                    playerCoef.PassingCoef = 0.9;
                    playerCoef.DribblingCoef = 0.9;
                    playerCoef.PhysicsCoef = 0.95;
                    break;

                case PlayerPosition.RightFlankDefender:
                case PlayerPosition.LeftFlankDefender:
                    playerCoef.SpeedCoef = 1;
                    playerCoef.StrikeCoef = 0.85;
                    playerCoef.DefendingCoef = 1;
                    playerCoef.PassingCoef = 1;
                    playerCoef.DribblingCoef = 1;
                    playerCoef.PhysicsCoef = 0.9;
                    break;

                case PlayerPosition.CentralDefensiveMidfielder:
                    playerCoef.SpeedCoef = 0.95;
                    playerCoef.StrikeCoef = 0.9;
                    playerCoef.DefendingCoef = 1;
                    playerCoef.PassingCoef = 1;
                    playerCoef.DribblingCoef = 1;
                    playerCoef.PhysicsCoef = 1;
                    break;

                case PlayerPosition.CentralMidfielder:
                    playerCoef.SpeedCoef = 0.9;
                    playerCoef.StrikeCoef = 0.9;
                    playerCoef.DefendingCoef = 0.9;
                    playerCoef.PassingCoef = 0.9;
                    playerCoef.DribblingCoef = 0.9;
                    playerCoef.PhysicsCoef = 0.9;
                    break;

                case PlayerPosition.CentralAttackingMidfielder:
                    playerCoef.SpeedCoef = 0.9;
                    playerCoef.StrikeCoef = 0.95;
                    playerCoef.DefendingCoef = 0.6;
                    playerCoef.PassingCoef = 1;
                    playerCoef.DribblingCoef = 1;
                    playerCoef.PhysicsCoef = 0.9;
                    break;

                case PlayerPosition.RightMidfielder:
                case PlayerPosition.LeftMidfielder:
                    playerCoef.SpeedCoef = 1;
                    playerCoef.StrikeCoef = 1;
                    playerCoef.DefendingCoef = 0.6;
                    playerCoef.PassingCoef = 1;
                    playerCoef.DribblingCoef = 1;
                    playerCoef.PhysicsCoef = 0.95;
                    break;

                case PlayerPosition.RightOffensive:
                case PlayerPosition.LeftOffensive:
                    playerCoef.SpeedCoef = 1;
                    playerCoef.StrikeCoef = 1;
                    playerCoef.DefendingCoef = 0.5;
                    playerCoef.PassingCoef = 1;
                    playerCoef.DribblingCoef = 1;
                    playerCoef.PhysicsCoef = 0.9;
                    break;

                case PlayerPosition.CentralForward:
                    playerCoef.SpeedCoef = 1;
                    playerCoef.StrikeCoef = 1;
                    playerCoef.DefendingCoef = 0.45;
                    playerCoef.PassingCoef = 1;
                    playerCoef.DribblingCoef = 1;
                    playerCoef.PhysicsCoef = 0.9;
                    break;

                case PlayerPosition.Forward:
                    playerCoef.SpeedCoef = 1;
                    playerCoef.StrikeCoef = 1;
                    playerCoef.DefendingCoef = 0.45;
                    playerCoef.PassingCoef = 0.95;
                    playerCoef.DribblingCoef = 1;
                    playerCoef.PhysicsCoef = 0.95;
                    break;
            }
            return playerCoef;
        }
    }
}
