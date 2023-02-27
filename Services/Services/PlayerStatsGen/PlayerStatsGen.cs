using FootBalLife.Database;
using System;
using System.Collections.Generic;

namespace FootBalLife.Services.PlayerStatsGen
{
    public class PlayerStatsGen
    {
        private readonly List<Player> _players;

        private const int _playersCount = 11;

        private readonly long _basicValue;
        private readonly long _errorValue;

        public PlayerStatsGen(long basicValue, long errorValue)
        {
            _basicValue = basicValue;
            _errorValue = errorValue;

            _players = new List<Player>(_playersCount);
        }
        public void Start()
        {
            _players.Clear();

            //var positions = Enum.GetValues<PlayerPosition>();
            var random = new Random();

            for (int i = 0; i < _playersCount; ++i)
            {
                var player = new Player
                {
                    //PersonID = i.ToString(),
                    //PositionCode = positions[random.Next(1, 3)],
                    /*Speed = ValueRandomGen.Next(_basicValue, _errorValue),
                    Strike = ValueRandomGen.Next(_basicValue, _errorValue),
                    Endurance = ValueRandomGen.Next(_basicValue, _errorValue),
                    Physics = ValueRandomGen.Next(_basicValue, _errorValue),
                    Technique = ValueRandomGen.Next(_basicValue, _errorValue),
                    Passing = ValueRandomGen.Next(_basicValue, _errorValue)*/
                };

                _players.Add(player);
            }

            //_players.First().PositionID = (long)PlayerPostion.GoalKeeper;
        }
        public List<Player> GetPlayersStats()
        {
            return _players;
        }
    }
}
