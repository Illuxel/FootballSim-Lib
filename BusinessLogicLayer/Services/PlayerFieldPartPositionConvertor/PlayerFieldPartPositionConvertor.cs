using BusinessLogicLayer.Rules;
using DatabaseLayer;
using DatabaseLayer.Enums;

namespace BusinessLogicLayer.Services
{
    public class PlayerFieldPartPositionConvertor
    {
        public PlayerFieldPartPosition Convert(PlayerPosition position)
        {
            switch (position)
            {
                case PlayerPosition.Goalkeeper:
                    return PlayerFieldPartPosition.Goalkeeper;
                case PlayerPosition.LeftDefender:
                case PlayerPosition.RightDefender:
                case PlayerPosition.CentralDefender:
                case PlayerPosition.RightFlankDefender:
                case PlayerPosition.LeftFlankDefender:
                    return PlayerFieldPartPosition.Defence;
                case PlayerPosition.RightMidfielder:
                case PlayerPosition.LeftMidfielder:
                case PlayerPosition.CentralMidfielder:
                case PlayerPosition.CentralDefensiveMidfielder:
                case PlayerPosition.CentralAttackingMidfielder:
                    return PlayerFieldPartPosition.Midfield;
                case PlayerPosition.Forward:
                case PlayerPosition.CentralForward:
                case PlayerPosition.LeftOffensive:
                case PlayerPosition.RightOffensive:
                    return PlayerFieldPartPosition.Attack;
                default:
                    return PlayerFieldPartPosition.All;
            }

        }
        public PlayerFieldPartPosition Convert(string positionCode)
        {
            var position = EnumDescription.GetEnumValueFromDescription<PlayerPosition>(positionCode);
            return Convert(position);
        }
    }
}
