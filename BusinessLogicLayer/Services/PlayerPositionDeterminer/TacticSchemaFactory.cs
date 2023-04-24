using DatabaseLayer;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    internal class TacticSchemaFactory
    {
        //TODO: add another scheme defenition
        public Dictionary<int, string> GetPlayersPosition(TacticSchema tacticSchema)
        {
            ISchemeDefinition strategy;
            switch (tacticSchema)
            {
                case TacticSchema.FourFourTwo:
                    strategy = new FourFourTwoSchemeDefenition();
                case tacticSchema.FourThreeThree:
                    strategy = new FourThreeThreeSchemeDefinition();
                case tacticSchema.FourFiveOne:
                    strategy = new FourFiveOneSchemeDefinition();
                case tacticSchema.ThreeFiveTwo:
                    strategy = new ThreeFiveTwoSchemeDefinition();
                case tacticSchema.FourOneFourOne:
                    strategy = new FourOneFourOneSchemeDefinition();
                case tacticSchema.FourFourOneOne:
                    strategy = new FourFourOneOneSchemeDefinition();
                default:
                    return strategy.GetPositions();
            }
            return null;
        }

    }
}
