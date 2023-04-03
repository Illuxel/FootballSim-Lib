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
                default:
                    strategy = new FourFourTwoSchemeDefenition();
                    return strategy.GetPositions();
            }
            return null;
        }

    }
}
