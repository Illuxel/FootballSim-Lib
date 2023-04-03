using BusinessLogicLayer.Rules;
using DatabaseLayer.Enums;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    internal interface ISchemeDefinition
    {
        Dictionary<int, string> GetPositions();
    }

    internal static class TacticSchemeNavigation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns>Key - same position (if empty key - its for all), value - changed avg rating of player</returns>
        internal static Dictionary<string, int> GetSamePosition(string positionCode)
        {
            var position = EnumDescription.GetEnumValueFromDescription<PlayerPosition>(positionCode);
            return GetSamePosition(position);
        }

        private static Dictionary<string, int> GetSamePosition(PlayerPosition position)
        {
            var result = new Dictionary<string, int>();

            //if dont find same position
            result.Add("", -20);
            switch (position)
            {
                case PlayerPosition.Goalkeeper:
                    result[""] = -40;
                    break;
                case PlayerPosition.CentralDefender:
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralDefensiveMidfielder), -5);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftDefender), -8);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightDefender), -8);
                    break;
                case PlayerPosition.LeftDefender:
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftFlankDefender), -2);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightDefender), -5);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftMidfielder), -7);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralDefender), -8);
                    break;
                case PlayerPosition.RightDefender:
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightFlankDefender), -2);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftDefender), -5);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightMidfielder), -7);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralDefender), -8);
                    break;
                case PlayerPosition.LeftFlankDefender:
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftDefender), -2);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftMidfielder), -4);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightFlankDefender), -5);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralDefensiveMidfielder), -7);
                    break;
                case PlayerPosition.RightFlankDefender:
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightDefender), -2);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightMidfielder), -4);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftFlankDefender), -5);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralDefensiveMidfielder), -7);
                    break;
                case PlayerPosition.CentralDefensiveMidfielder:
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralMidfielder), -2);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralDefender), -5);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftFlankDefender), -7);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightFlankDefender), -7);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralAttackingMidfielder), -8);
                    break;
                case PlayerPosition.CentralMidfielder:
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralDefensiveMidfielder), -2);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralAttackingMidfielder), -3);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftMidfielder), -5);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightMidfielder), -5);
                    break;
                case PlayerPosition.LeftMidfielder:
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftOffensive), -3);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftFlankDefender), -4);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralAttackingMidfielder), -4);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralMidfielder), -5);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightMidfielder), -5);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftDefender), -7);
                    break;
                case PlayerPosition.RightMidfielder:
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightOffensive), -3);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightFlankDefender), -4);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralAttackingMidfielder), -4);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralMidfielder), -5);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftMidfielder), -5);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightDefender), -7);
                    break;
                case PlayerPosition.CentralAttackingMidfielder:
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralMidfielder), -3);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftMidfielder), -4);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightMidfielder), -4);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.Forward), -4);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralMidfielder), -5);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralForward), -7);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralDefensiveMidfielder), -8);
                    break;
                case PlayerPosition.LeftOffensive:
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftMidfielder), -3);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightOffensive), -3);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.Forward), -4);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralForward), -5);
                    break;
                case PlayerPosition.RightOffensive:
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightMidfielder), -3);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftOffensive), -3);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.Forward), -4);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralForward), -5);
                    break;
                case PlayerPosition.Forward:
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralForward), -2);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightOffensive), -4);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftOffensive), -4);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralAttackingMidfielder), -4);
                    break;
                case PlayerPosition.CentralForward:
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.Forward), -2);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.RightOffensive), -5);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.LeftOffensive), -5);
                    result.Add(EnumDescription.GetEnumDescription(PlayerPosition.CentralAttackingMidfielder), -7);
                    break;
            }
            return result;
        }
    }
}
