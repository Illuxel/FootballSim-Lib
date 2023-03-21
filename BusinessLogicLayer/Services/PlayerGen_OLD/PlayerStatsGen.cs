using DatabaseLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class PlayerStatsGen_old
    {
        private readonly List<Player> _players;

        private const int _playersCount = 11;

        private readonly int _basicValue;
        private readonly int _errorValue;

        public PlayerStatsGen_old(int basicValue, int errorValue)
        {
            _basicValue = basicValue;
            _errorValue = errorValue;

            _players = new List<Player>(_playersCount);
        }
        public void Start()
        {
            _players.Clear();

            var rand = new Random();
            List<PlayerPosition> pos = new List<PlayerPosition>();
            pos.Add(PlayerPosition.Goalkeeper);
            pos.Add(PlayerPosition.Attack);
            pos.Add(PlayerPosition.Defence);
            pos.Add(PlayerPosition.Midfield);
            for (int i = 0; i < _playersCount; ++i)
            {

                var player = new Player
                {
                    PersonID = Guid.NewGuid().ToString(),
                    Position = new Position() {
                        Location = pos[rand.Next(2, pos.Count())]
                    },
                    Speed = ValueRandomGen.Next(_basicValue, _errorValue, 20),
                    Strike = ValueRandomGen.Next(_basicValue, _errorValue, 20),
                    Endurance = ValueRandomGen.Next(_basicValue, _errorValue, 20),
                    Physics = ValueRandomGen.Next(_basicValue, _errorValue, 20),
                    Dribbling = ValueRandomGen.Next(_basicValue, _errorValue, 20),
                    Passing = ValueRandomGen.Next(_basicValue, _errorValue, 20),
                    Defending = ValueRandomGen.Next(_basicValue, _errorValue, 20)
                };

                _players.Add(player);
            }

            _players[0].Position.Location = PlayerPosition.Goalkeeper;

            Console.WriteLine("Attackers: " + _players.Where(p => p.Position.Location == PlayerPosition.Attack).Count());
            Console.WriteLine("Def: " + _players.Where(p => p.Position.Location == PlayerPosition.Defence).Count());
            Console.WriteLine("Mid: " + _players.Where(p => p.Position.Location == PlayerPosition.Midfield).Count());
        }
        public List<Player> GetPlayers()
        {
            return _players;
        }
    }
}
