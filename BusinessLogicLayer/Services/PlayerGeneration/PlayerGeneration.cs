using DatabaseLayer;
using DatabaseLayer.Enums;
using System;

namespace BusinessLogicLayer.Services.PlayerGeneration
{
    public class PlayerGeneration
    {
        private Player _Player = new Player();
        private PlayerCoefPropertyFactory _PlayerCoefPropertyFactory;
        private int _error = 3, _maxValue = 100, _minValue = 1;

        public PlayerGeneration()
        {
            _PlayerCoefPropertyFactory = new PlayerCoefPropertyFactory();
        }

        public Player GeneratePlayer()
        {          
            return _Player;
        }

        public Player GeneratePlayer(PlayerPosition position, int value)
        {
            var playersCoef = _PlayerCoefPropertyFactory.Create(position);
            var gaussianGenerator = new GaussianGeneration(value, _error, _minValue, _maxValue);
            PlayerProperties(playersCoef, gaussianGenerator);

            return _Player;
        }

        public Player GeneratePlayer(int value)
        {            
            Random random = new Random();
            PlayerPosition[] enumPositionValues = (PlayerPosition[])Enum.GetValues(typeof(PlayerPosition));
            int randomIndex = random.Next(0, enumPositionValues.Length);
            PlayerPosition randomPosition = enumPositionValues[randomIndex];
            var playersCoef = _PlayerCoefPropertyFactory.Create(randomPosition);
            var gaussianGenerator = new GaussianGeneration(value, _error, _minValue, _maxValue);
            PlayerProperties(playersCoef, gaussianGenerator);

            return _Player;
        }

        private void PlayerProperties(PlayerCoefImportanceProperty playersCoef, GaussianGeneration gaussianGenerator)
        {
            _Player.Strike = Convert.ToInt32(gaussianGenerator.Next() * playersCoef.StrikeCoef);
            _Player.Speed = Convert.ToInt32(gaussianGenerator.Next() * playersCoef.SpeedCoef);
            _Player.Physics = Convert.ToInt32(gaussianGenerator.Next() * playersCoef.PhysicsCoef);
            _Player.Defending = Convert.ToInt32(gaussianGenerator.Next() * playersCoef.DefendingCoef);
            _Player.Passing = Convert.ToInt32(gaussianGenerator.Next() * playersCoef.PassingCoef);
            _Player.Dribbling = Convert.ToInt32(gaussianGenerator.Next() * playersCoef.DribblingCoef);
            _Player.Rating = CalculateRating(_Player, playersCoef);
        }

        private int CalculateRating(Player player, PlayerCoefImportanceProperty playersCoef)
        {
            return Convert.ToInt32((player.Strike / playersCoef.StrikeCoef + player.Speed / playersCoef.SpeedCoef +
                player.Physics / playersCoef.PhysicsCoef + player.Defending / playersCoef.DefendingCoef +
                player.Passing / playersCoef.PassingCoef + player.Dribbling / playersCoef.DribblingCoef) / 6);
        }
    }

}
