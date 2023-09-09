using DatabaseLayer.Enums;

namespace DatabaseLayer.Services
{
    public class PositionNameGetter
    {
        public string Get(PlayerPosition position)
        {
            switch (position)
            {
                case PlayerPosition.Goalkeeper:
                    return "Goalkeeper";
                case PlayerPosition.CentralDefender:
                    return "Central defender";
                case PlayerPosition.LeftDefender:
                    return "Left defender";
                case PlayerPosition.RightDefender:
                    return "Right defender";
                case PlayerPosition.LeftFlankDefender:
                    return "Left flank defender";
                case PlayerPosition.RightFlankDefender:
                    return "Right flank defender";
                case PlayerPosition.CentralDefensiveMidfielder:
                    return "Central defensive midfielder";
                case PlayerPosition.CentralMidfielder:
                    return "Central midfielder";
                case PlayerPosition.LeftMidfielder:
                    return "Left midfielder";
                case PlayerPosition.RightMidfielder:
                    return "Right midfielder";
                case PlayerPosition.CentralAttackingMidfielder:
                    return "Central attacking midfielder";
                case PlayerPosition.LeftOffensive:
                    return "Left offensive";
                case PlayerPosition.RightOffensive:
                    return "Right offensive";
                case PlayerPosition.Forward:
                    return "Forward";
                case PlayerPosition.CentralForward:
                    return "Central forward";
            }

            return string.Empty;
        }
    }
}
