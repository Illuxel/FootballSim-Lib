using DatabaseLayer;
using System;

namespace BusinessLogicLayer.Services
{
    public class PlayerPriceCalculator
    {
        private double _ageCoefficient = 0.8;
        private int _age = 25;
        private int _coefPrice = 25;
        public int GetPlayerPrice(Player player)
        {
            var avaragePlayerRating = player.CurrentPlayerRating;
            if (avaragePlayerRating >= 90)
            {
                return avaragePlayerRating * (100000 * _coefPrice);
            }
            else if (avaragePlayerRating >= 85)
            {
                return avaragePlayerRating * (65000* _coefPrice);
            }
            else if (avaragePlayerRating >= 80)
            {
                return avaragePlayerRating * (45000* _coefPrice);
            }
            else if (avaragePlayerRating >= 75)
            {
                return avaragePlayerRating * (30000 * _coefPrice);
            }
            else if (avaragePlayerRating >= 70)
            {
                return avaragePlayerRating * (20000 * _coefPrice);
            }
            else if (avaragePlayerRating >= 60)
            {
                return avaragePlayerRating * (7000 * _coefPrice);
            }
            return avaragePlayerRating * (4500 * _coefPrice);
        }

        public int GetPlayerSalary(Player player)
        {
            var age = Convert.ToInt32((DateTime.Now - player.Person.Birthday).TotalDays / 365.24);
            var price = GetPlayerPrice(player) / _coefPrice;

            var ageDifference = _age - age;
            _ageCoefficient += ageDifference * 0.1;

            Random random = new Random();
            var randomCorrelation = random.NextDouble() * 0.2 - 0.1;
            var priceWithAge = price * _ageCoefficient;
            var finalPrice = (int)(priceWithAge + (priceWithAge * randomCorrelation));

            return finalPrice;
        }
    }
}
