using BusinessLogicLayer.Rules;
using DatabaseLayer.Enums;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    internal class FourFourTwoSchemeDefenition : ISchemeDefinition
    {
        //index by positions:
        //-----1-----
        //--5-4-3-2--
        //-16-15-13-12-
        //--25---26--
        public Dictionary<int, string> GetPositions()
        {
            return new Dictionary<int, string>()
            {
                { 1, EnumDescription.GetEnumDescription(PlayerPosition.Goalkeeper) },
                { 5, EnumDescription.GetEnumDescription(PlayerPosition.RightDefender) },
                { 4, EnumDescription.GetEnumDescription(PlayerPosition.CentralDefender) },
                { 3, EnumDescription.GetEnumDescription(PlayerPosition.CentralDefender) },
                { 2, EnumDescription.GetEnumDescription(PlayerPosition.LeftDefender) },
                { 16, EnumDescription.GetEnumDescription(PlayerPosition.RightMidfielder) },
                { 15, EnumDescription.GetEnumDescription(PlayerPosition.CentralMidfielder) },
                { 13, EnumDescription.GetEnumDescription(PlayerPosition.CentralMidfielder) },
                { 12, EnumDescription.GetEnumDescription(PlayerPosition.LeftMidfielder) },
                { 25, EnumDescription.GetEnumDescription(PlayerPosition.Forward) },
                { 26, EnumDescription.GetEnumDescription(PlayerPosition.Forward) }
            };
        }
       
    }
}
