using DatabaseLayer;
using System;

namespace BusinessLogicLayer.Services
{
    public class PlayerInjuryFinder
    {
        private static int _criticalEnduranceLevel = 40;
        private bool isMayBeInjuried(Player player)
        {
            if (player.Endurance < _criticalEnduranceLevel)
            {
                return true;
            }
            return false;
        }
        public bool IsInjuried(Player player)
        {
            if (isMayBeInjuried(player))
            {
                if (isWillBeInjuried(player))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsAlreadyInjuried(Player player)
        {
            if (player.InjuredTo != null)
            {
                return true;
            }
            return false;
        }
        private bool isWillBeInjuried(Player player)
        {
            var injuryChance = _criticalEnduranceLevel - player.Endurance;

            var randomNum = new Random().Next(0, 100);
            if (randomNum <= injuryChance)
            {
                return true;
            }
            return false;
        }

        public void SetInjury(Player player, DateTime gameDate)
        {
            var playerInjuryTerm = getInjuryTermDays();

            player.InjuredTo = gameDate.AddDays(playerInjuryTerm).ToString("yyyy-MM-dd");
        }
        private int getInjuryTermDays()
        {
            var randomNum = new Random().Next(0, 100);

            if (randomNum < 60)
            {
                return 14;
            }
            else if (randomNum < 85)
            {
                return 30;
            }
            else if (randomNum < 95)
            {
                return 90;
            }
            else
            {
                return 180;
            }
        }
    }
}
