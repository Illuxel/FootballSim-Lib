using DatabaseLayer;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Numerics;

namespace BusinessLogicLayer.Services
{
    internal class TeamForMatch : ITeamForMatch
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BaseColor { get; set; }

        public int AvailablePlayerCount { get; set; }
        public StrategyType Strategy { get; set; }
        public TacticSchema TacticSchema { get; set; }
        public List<Player> AllPlayers { get; set; }
        public Dictionary<int, TacticPlayerPosition> MainPlayers { get; set; }

        public List<Player> SparePlayers { get; set; }

        //Instance for spared players list
        public List<Player> SparedPlayers { get; set; }


        private PlayerPositionDeterminer _playerPositionDeterminer { get; set; }

        internal TeamForMatch()
         {

            AllPlayers = new List<Player>();
            MainPlayers = new Dictionary<int, TacticPlayerPosition>();
            SparePlayers = new List<Player>();
            SparedPlayers = new List<Player>();

            _playerPositionDeterminer = new PlayerPositionDeterminer();
            AvailablePlayerCount = 11;
            MainPlayers = _playerPositionDeterminer.GetPlayersWithPosition(TacticSchema, AllPlayers);
         }

         public void ChangeTacticScheme(TacticSchema newTacticSchema)
         {
             TacticSchema = newTacticSchema;
             MainPlayers = _playerPositionDeterminer.GetPlayersWithPosition(newTacticSchema,
                 MainPlayers.Values.Select(item => item.CurrentPlayer).ToList());
         }
        //Implemented SubstitutePlayer method
        public void SubstitutePlayer(int indexMainPlayer, Player sparePlayer)
        {
            if(MainPlayers.TryGetValue(indexMainPlayer,out var playerPosition))
            {
                SparedPlayers.Add(playerPosition.CurrentPlayer);
                playerPosition.CurrentPlayer = sparePlayer;
            }
            else
            {
                throw new System.Exception("Index doesn`t belong to that team");
            }
        }

        public Player GetPlayer(PlayerFieldPartPosition playerPostion)
        {
            var selectedPlayers = MainPlayers.Where(player => player.Value.CurrentPlayer != null && player.Value.FieldPosition == playerPostion);
            return selectedPlayers.Select(item => item.Value.CurrentPlayer).FirstOrDefault();
        }
        public double AvgSpeed(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            return playerPostion == PlayerFieldPartPosition.All
                ? MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Speed)
            : MainPlayers
                    .Where(p => p.Value.CurrentPlayer != null && p.Value.FieldPosition == playerPostion)
                    .Average(p => p.Value.CurrentPlayer.Speed);
        }
        public double AvgStrike(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            return playerPostion == PlayerFieldPartPosition.All
                ? MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Strike)
                : MainPlayers
                    .Where(p => p.Value.CurrentPlayer != null && p.Value.FieldPosition == playerPostion)
                    .Average(p => p.Value.CurrentPlayer.Strike);
        }
        public double AvgDefense(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            return playerPostion == PlayerFieldPartPosition.All
                ? MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Defending)
                : MainPlayers
                    .Where(p => p.Value.CurrentPlayer != null && p.Value.FieldPosition == playerPostion)
                    .Average(p => p.Value.CurrentPlayer.Defending);
        }
        public double AvgPhysicalTraining(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            return playerPostion == PlayerFieldPartPosition.All
                ? MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Physics)
                : MainPlayers
                    .Where(p => p.Value.CurrentPlayer != null && p.Value.FieldPosition == playerPostion)
                    .Average(p => p.Value.CurrentPlayer.Physics);
        }
        public double AvgTechnique(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            return playerPostion == PlayerFieldPartPosition.All
                ? MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Dribbling)
                : MainPlayers
                    .Where(p => p.Value.FieldPosition == playerPostion)
                    .Average(p => p.Value.CurrentPlayer.Dribbling);
        }
        public double AvgPassing(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            return playerPostion == PlayerFieldPartPosition.All
                ? MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Passing)
                : MainPlayers
                    .Where(p => p.Value.CurrentPlayer != null && p.Value.FieldPosition == playerPostion)
                    .Average(p => p.Value.CurrentPlayer.Passing);
        }

    }
}
