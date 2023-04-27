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
                    break;
                case TacticSchema.FourThreeThree:
                    strategy = new FourThreeThreeSchemeDefenition();
                    break;
                case TacticSchema.FourFiveOne:
                    strategy = new FourFiveOneSchemeDefenition();
                    break;
                case TacticSchema.ThreeFiveTwo:
                    strategy = new ThreeFiveTwoSchemeDefenition(); 
                    break;
                case TacticSchema.FourOneFourOne:
                    strategy = new FourOneFourOneSchemeDefenition();   
                    break;
                case TacticSchema.FourFourOneOne:
                    strategy = new FourFourOneOneSchemeDefenition();
                    break;
                default:
                    strategy = new FourFourTwoSchemeDefenition();
                    return strategy.GetPositions();
            }
            return null;
        }

    }
}
