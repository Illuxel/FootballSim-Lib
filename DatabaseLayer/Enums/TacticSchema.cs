using System.ComponentModel;

namespace DatabaseLayer
{
    public enum TacticSchema
    {
        [Description("4-3-3")]
        FourThreeThree,
        [Description("4-4-2")]
        FourFourTwo,
        [Description("4-5-1")]
        FourFiveOne,
        [Description("3-5-2")]
        ThreeFiveTwo,
        [Description("4-1-4-1")]
        FourOneFourOne,
        [Description("4-4-1-1")]
        FourFourOneOne
    }
}
