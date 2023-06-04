using DatabaseLayer.Enums;

namespace DatabaseLayer.Services
{
    public class ScoutLevelUpgrader
    {
        public bool Upgrade(ScoutSkillLevel level, PlayerGameData playerGameData)
        {

            double upgradeCost = GetUpgradeCost(level);

            if (playerGameData.Money >= upgradeCost)
            {
            
                playerGameData.level = level;
                playerGameData.Money -= upgradeCost;

                return true;
            }

            return false;
        }

        private double GetUpgradeCost(ScoutSkillLevel level)
        {
            switch (level)
            {
                case ScoutSkillLevel.Level2:
                    return 3000;
                case ScoutSkillLevel.Level3:
                    return 5000;
                case ScoutSkillLevel.Level4:
                    return 10000;
                default:
                    return 0; 
            }
        }
    }

}
