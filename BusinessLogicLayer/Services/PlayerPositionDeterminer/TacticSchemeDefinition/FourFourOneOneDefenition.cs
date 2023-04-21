using BusinessLogicLayer.Rules;
using DatabaseLayer.Enums;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    internal class FourFourOneOneDefenition : ISchemeDefinition
    {
        //index by positions:
        //-----1-----
        //--2-3-4-5--
        //--6-7-8-9--
        //----10-----
        //----11-----
        public Dictionary<int, string> GetPositions()
        {
            return new Dictionary<int, string>()
            {
                { 1, EnumDescription.GetEnumDescription(PlayerPosition.Goalkeeper) },
                { 2, EnumDescription.GetEnumDescription(PlayerPosition.RightDefender) },
                { 3, EnumDescription.GetEnumDescription(PlayerPosition.CentralDefender) },
                { 4, EnumDescription.GetEnumDescription(PlayerPosition.CentralDefender) },
                { 5, EnumDescription.GetEnumDescription(PlayerPosition.LeftDefender) },
                { 6, EnumDescription.GetEnumDescription(PlayerPosition.RightMidfielder) },
                { 7, EnumDescription.GetEnumDescription(PlayerPosition.CentralMidfielder) },
                { 8, EnumDescription.GetEnumDescription(PlayerPosition.CentralMidfielder) },
                { 9, EnumDescription.GetEnumDescription(PlayerPosition.LeftMidfielder) },
                { 10, EnumDescription.GetEnumDescription(PlayerPosition.CentralAttackingMidfielder) },
                { 11, EnumDescription.GetEnumDescription(PlayerPosition.Forward) }
            };
        }
       
    }
}
