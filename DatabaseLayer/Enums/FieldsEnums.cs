using System.ComponentModel;

namespace DatabaseLayer
{
    public enum PlayerFieldPartPosition
    {
        All,
        Goalkeeper,
        Defence,
        Midfield,
        Attack
    }
    public enum StrategyType
    {
        [Description("Ball Сontrol")]
        BallСontrol,
        [Description("Long ball")]
        DefenseAttack,
        [Description("Total Pressing")]
        TotalPressing,
        [Description("Total Defense")]
        TotalDefense
    }
}
