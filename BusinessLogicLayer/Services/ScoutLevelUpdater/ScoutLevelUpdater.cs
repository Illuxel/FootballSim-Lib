using DatabaseLayer.Enums;
using DatabaseLayer.Services;

namespace BusinessLogicLayer.Services
{
    public class ScoutLevelUpgrader
    {
        public bool Upgrade(ScoutSkillLevel level, PlayerGameData playerGameData)
        {

            double upgradeCost = getUpgradeCost(level);

            if (playerGameData.Money >= upgradeCost)
            {
            
                playerGameData.CurrentLevel = level;
                playerGameData.Money -= upgradeCost;

                return true;
            }

            return false;
        }

        private double getUpgradeCost(ScoutSkillLevel level)
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
