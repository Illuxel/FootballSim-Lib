using BusinessLogicLayer.Rules;
using DatabaseLayer.Enums;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    internal class FourOneFourOneSchemeDefenition : ISchemeDefinition
    {
        //index by positions:
        //-----1-----
        //--5-4-3-2--
        //-----9-----
        //-16-19-17-12-
        //-----22------
        public Dictionary<int, string> GetPositions()
        {
            return new Dictionary<int, string>()
            {
                { 1, EnumDescription.GetEnumDescription(PlayerPosition.Goalkeeper) },
                { 5, EnumDescription.GetEnumDescription(PlayerPosition.RightDefender) },
                { 4, EnumDescription.GetEnumDescription(PlayerPosition.CentralDefender) },
                { 3, EnumDescription.GetEnumDescription(PlayerPosition.CentralDefender) },
                { 2, EnumDescription.GetEnumDescription(PlayerPosition.LeftDefender) },
                { 9, EnumDescription.GetEnumDescription(PlayerPosition.CentralDefensiveMidfielder) },
                { 12, EnumDescription.GetEnumDescription(PlayerPosition.LeftMidfielder) },
                { 17, EnumDescription.GetEnumDescription(PlayerPosition.CentralAttackingMidfielder) },
                { 19, EnumDescription.GetEnumDescription(PlayerPosition.CentralAttackingMidfielder) },
                { 16, EnumDescription.GetEnumDescription(PlayerPosition.RightMidfielder) },
                { 22, EnumDescription.GetEnumDescription(PlayerPosition.Forward) }
            };
        }
       
    }
}
