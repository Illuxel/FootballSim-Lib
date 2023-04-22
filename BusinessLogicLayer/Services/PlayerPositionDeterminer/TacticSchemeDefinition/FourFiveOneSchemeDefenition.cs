using BusinessLogicLayer.Rules;
using DatabaseLayer.Enums;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    internal class FourFiveOneSchemeDefenition : ISchemeDefinition
    {
        //index by positions:
        //-------1------
        //----2-3-5-6----
        //-12-13-14-15-16-
        //------22-------
        public Dictionary<int, string> GetPositions()
        {
            return new Dictionary<int, string>()
            {
                { 1, EnumDescription.GetEnumDescription(PlayerPosition.Goalkeeper) },
                { 2, EnumDescription.GetEnumDescription(PlayerPosition.RightDefender) },
                { 3, EnumDescription.GetEnumDescription(PlayerPosition.CentralDefender) },
                { 5, EnumDescription.GetEnumDescription(PlayerPosition.CentralDefender) },
                { 6, EnumDescription.GetEnumDescription(PlayerPosition.LeftDefender) },
                { 12, EnumDescription.GetEnumDescription(PlayerPosition.RightMidfielder) },
                { 13, EnumDescription.GetEnumDescription(PlayerPosition.CentralMidfielder) },
                { 14, EnumDescription.GetEnumDescription(PlayerPosition.CentralMidfielder) },
                { 15, EnumDescription.GetEnumDescription(PlayerPosition.CentralMidfielder },
                { 16, EnumDescription.GetEnumDescription(PlayerPosition.LeftMidfilder) },
                { 22, EnumDescription.GetEnumDescription(PlayerPosition.Forward) }
            };
        }
       
    }
}
