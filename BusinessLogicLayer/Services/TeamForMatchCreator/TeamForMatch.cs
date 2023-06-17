using DatabaseLayer;
using System.Collections.Generic;
using System.Linq;

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
            if(selectedPlayers.Count() == 0)
            {
                var players = MainPlayers.Where(player => player.Value.CurrentPlayer != null).Select(x=>x.Value.CurrentPlayer);
                var random = new System.Random();
                return players.ElementAt(random.Next(0,players.Count() - 1));
            }
            return selectedPlayers.Select(item => item.Value.CurrentPlayer).FirstOrDefault();
        }
        public double AvgSpeed(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            return playerPostion == PlayerFieldPartPosition.All
                ? MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Speed)
            : getPlayersByPosition(playerPostion).Average(p => p.Speed);
        }
        public double AvgStrike(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            return playerPostion == PlayerFieldPartPosition.All
                ? MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Strike)
                : getPlayersByPosition(playerPostion).Average(p => p.Strike);
        }

        public double AvgDefense(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            return playerPostion == PlayerFieldPartPosition.All
                ? MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Defending)
                : getPlayersByPosition(playerPostion).Average(p => p.Defending);
        }
        public double AvgPhysicalTraining(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            return playerPostion == PlayerFieldPartPosition.All
                ? MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Physics)
                : getPlayersByPosition(playerPostion).Average(p => p.Physics);
        }
        public double AvgTechnique(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            return playerPostion == PlayerFieldPartPosition.All
                ? MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Dribbling)
                : getPlayersByPosition(playerPostion).Average(p => p.Dribbling);
        }
        public double AvgPassing(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            return playerPostion == PlayerFieldPartPosition.All
                ? MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Passing)
                : getPlayersByPosition(playerPostion).Average(p => p.Passing);
        }

        private List<Player> getPlayersByPosition(PlayerFieldPartPosition playerPostion)
        {
            var players = MainPlayers.Where(player => player.Value.CurrentPlayer != null && player.Value.FieldPosition == playerPostion)
                .Select(item => item.Value.CurrentPlayer).ToList();
            if(players.Count == 0)
            {
                players = MainPlayers.Where(player => player.Value.CurrentPlayer != null && player.Value.FieldPosition != playerPostion)
                .Select(item => item.Value.CurrentPlayer).ToList();
            }
            return players;
        }

    }
}
