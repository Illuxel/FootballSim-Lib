using DatabaseLayer;
using DatabaseLayer.Enums;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace BusinessLogicLayer.Services.PlayerGeneration
{
    public class PlayerGeneration
    {
        Player _Player;
        PlayerCoefPropertyFactory _PlayerCoefPropertyFactory;
        public int error = 3, maxValue = 100, minValue = 1;
        public void GeneratePlayer()
        {
            _Player= new Player();
            _PlayerCoefPropertyFactory = new PlayerCoefPropertyFactory();
        }
                          
        public void GeneratePlayer(PlayerPosition position, int value)//int value має бути значенням середього рівня академії в подальшому
        {
            var playersCoef = _PlayerCoefPropertyFactory.Create(position);

            _Player.Strike    = Convert.ToInt32(new GaussianGeneration(value, error, minValue, maxValue).Next() * playersCoef.StrikeCoef);
            _Player.Speed     = Convert.ToInt32(new GaussianGeneration(value, error, minValue, maxValue).Next() * playersCoef.SpeedCoef);
            _Player.Physics   = Convert.ToInt32(new GaussianGeneration(value, error, minValue, maxValue).Next() * playersCoef.PhysicsCoef);
            _Player.Defending = Convert.ToInt32(new GaussianGeneration(value, error, minValue, maxValue).Next() * playersCoef.DefendingCoef);
            _Player.Passing   = Convert.ToInt32(new GaussianGeneration(value, error, minValue, maxValue).Next() * playersCoef.PassingCoef);
            _Player.Dribbling = Convert.ToInt32(new GaussianGeneration(value, error, minValue, maxValue).Next() * playersCoef.DribblingCoef);
            _Player.Rating    = Convert.ToInt32((_Player.Strike / playersCoef.StrikeCoef + _Player.Speed / playersCoef.SpeedCoef +
                _Player.Physics / playersCoef.PhysicsCoef + _Player.Defending / playersCoef.DefendingCoef +
                _Player.Passing / playersCoef.PassingCoef + _Player.Dribbling / playersCoef.DribblingCoef) / 6);                    
        }
        public void GeneratePlayer(int value)
        {
            Random random = new Random();           
            PlayerPosition[] enumPositionValues = (PlayerPosition[])Enum.GetValues(typeof(PlayerPosition));
            int randomIndex = random.Next(0, enumPositionValues.Length);
            PlayerPosition randomPosition = enumPositionValues[randomIndex];
            var playersCoef = _PlayerCoefPropertyFactory.Create(randomPosition);
            _Player.Speed     = Convert.ToInt32(new GaussianGeneration(value, error, minValue, maxValue).Next() * playersCoef.SpeedCoef);
            _Player.Strike    = Convert.ToInt32(new GaussianGeneration(value, error, minValue, maxValue).Next() * playersCoef.StrikeCoef);
            _Player.Defending = Convert.ToInt32(new GaussianGeneration(value, error, minValue, maxValue).Next() * playersCoef.DefendingCoef);
            _Player.Passing   = Convert.ToInt32(new GaussianGeneration(value, error, minValue, maxValue).Next() * playersCoef.PassingCoef);
            _Player.Dribbling = Convert.ToInt32(new GaussianGeneration(value, error, minValue, maxValue).Next() * playersCoef.DribblingCoef);
            _Player.Physics   = Convert.ToInt32(new GaussianGeneration(value, error, minValue, maxValue).Next() * playersCoef.PhysicsCoef);
            _Player.Rating    = Convert.ToInt32((_Player.Strike / playersCoef.StrikeCoef + _Player.Speed / playersCoef.SpeedCoef +
                _Player.Physics / playersCoef.PhysicsCoef + _Player.Defending / playersCoef.DefendingCoef +
                _Player.Passing / playersCoef.PassingCoef + _Player.Dribbling / playersCoef.DribblingCoef) / 6);            
        }
    }
}
