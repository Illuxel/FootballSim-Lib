using DatabaseLayer;
using DatabaseLayer.Enums;
using System;
using BusinessLogicLayer.Rules;

namespace BusinessLogicLayer.Services
{
    public class PlayerGeneration
    {
        private PlayerCoefPropertyFactory _PlayerCoefPropertyFactory;
        
        private int _error = 3, _maxValue = 100, _minValue = 1;

        public PlayerGeneration()
        {
            _PlayerCoefPropertyFactory = new PlayerCoefPropertyFactory();
        }       

        public Player GeneratePlayer(PlayerPosition position, int value)
        {
            return generatePlayer(position, value);
        }

        public Player GeneratePlayer(int value)
        {
            return generatePlayer(GetRandomPlayerPosition(), value);
        }

        public void SetMaxMinValue(int maxValue, int minValue)
        {
            _maxValue = maxValue;
            _minValue = minValue;
        }
        private Player generatePlayer(PlayerPosition position, int value = 0)
        {
            Player player = new Player();
            

            PlayerPosition playerPosition = position;
            var playersCoef = _PlayerCoefPropertyFactory.Create(position);
            var gaussianGenerator = new GaussianGeneration(value, _error, _minValue, _maxValue);

            player.Strike = Convert.ToInt32(gaussianGenerator.Next() * playersCoef.StrikeCoef);
            player.Speed = Convert.ToInt32(gaussianGenerator.Next() * playersCoef.SpeedCoef);
            player.Physics = Convert.ToInt32(gaussianGenerator.Next() * playersCoef.PhysicsCoef);
            player.Defending = Convert.ToInt32(gaussianGenerator.Next() * playersCoef.DefendingCoef);
            player.Passing = Convert.ToInt32(gaussianGenerator.Next() * playersCoef.PassingCoef);
            player.Dribbling = Convert.ToInt32(gaussianGenerator.Next() * playersCoef.DribblingCoef);
            player.Rating = CalculateAverageRating(player, playersCoef);
            player.PositionCode = EnumDescription.GetEnumDescription(position);

            return player;
        }

        private PlayerPosition GetRandomPlayerPosition()
        {
            Random random = new Random();
            PlayerPosition[] enumPositionValues = (PlayerPosition[])Enum.GetValues(typeof(PlayerPosition));
            int randomIndex = random.Next(0, enumPositionValues.Length);
            PlayerPosition randomPosition = enumPositionValues[randomIndex];
            return randomPosition;
        }

        internal int CalculateAverageRating(Player player, PlayerCoefImportanceProperty playersCoef)
        {
            return Convert.ToInt32((player.Strike / playersCoef.StrikeCoef + player.Speed / playersCoef.SpeedCoef +
                player.Physics / playersCoef.PhysicsCoef + player.Defending / playersCoef.DefendingCoef +
                player.Passing / playersCoef.PassingCoef + player.Dribbling / playersCoef.DribblingCoef) / 6);
        }
    }
}