using System.ComponentModel;

namespace DatabaseLayer.Enums
{
    public enum PlayerPosition
    {
        [Description("GK")]
        Goalkeeper,

        [Description("CB")]
        CentralDefender,

        [Description("RB")]
        RightDefender,

        [Description("LB")]
        LeftDefender,

        [Description("RWB")]
        RightFlankDefender,

        [Description("LWB")]
        LeftFlankDefender,

        [Description("CDM")]
        CentralDefensiveMidfielder,

        [Description("CM")]
        CentralMidfielder,

        [Description("RM")]
        RightMidfielder,

        [Description("LM")]
        LeftMidfielder,

        [Description("CAM")]
        CentralAttackingMidfielder,

        [Description("ST")]
        Forward,

        [Description("CF")]
        CentralForward,

        [Description("RW")]
        RightOffensive,

        [Description("LW")]
        LeftOffensive

    }

}
