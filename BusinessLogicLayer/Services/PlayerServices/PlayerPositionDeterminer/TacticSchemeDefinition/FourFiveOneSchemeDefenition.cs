using BusinessLogicLayer.Rules;
using DatabaseLayer.Enums;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    internal class FourFiveOneSchemeDefenition : ISchemeDefinition
    {
        //index by positions:
        //-------1------
        //----6-5-3-2----
        //-16-15-14-13-12-
        //------22-------
        public Dictionary<int, string> GetPositions()
        {
            return new Dictionary<int, string>()
            {
                { 1, EnumDescription.GetEnumDescription(PlayerPosition.Goalkeeper) },
                { 6, EnumDescription.GetEnumDescription(PlayerPosition.RightDefender) },
                { 5, EnumDescription.GetEnumDescription(PlayerPosition.CentralDefender) },
                { 3, EnumDescription.GetEnumDescription(PlayerPosition.CentralDefender) },
                { 2, EnumDescription.GetEnumDescription(PlayerPosition.LeftDefender) },
                { 16, EnumDescription.GetEnumDescription(PlayerPosition.RightMidfielder) },
                { 15, EnumDescription.GetEnumDescription(PlayerPosition.CentralMidfielder) },
                { 14, EnumDescription.GetEnumDescription(PlayerPosition.CentralMidfielder) },
                { 13, EnumDescription.GetEnumDescription(PlayerPosition.CentralMidfielder) },
                { 12, EnumDescription.GetEnumDescription(PlayerPosition.LeftMidfielder) },
                { 22, EnumDescription.GetEnumDescription(PlayerPosition.Forward) }
            };
        }

    }
}
