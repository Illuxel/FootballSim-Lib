using DatabaseLayer;
using System;
using System.Collections.Generic;
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

        public List<PlayerInMatch> PlayersInMatch { get; set; }
        public List<Player> SparePlayers { get; set; }

        //Instance for spared players list
        public List<Player> SparedPlayers { get; set; }

        private int _totalMainPlayers = 11;


        private PlayerPositionDeterminer _playerPositionDeterminer { get; set; }

        internal TeamForMatch(string teamId,List<Player> allPlayers)
        {
            Id = teamId;
            PlayersInMatch = new List<PlayerInMatch>();
            AllPlayers = allPlayers;
            MainPlayers = new Dictionary<int, TacticPlayerPosition>();
            SparePlayers = new List<Player>();
            SparedPlayers = new List<Player>();

            _playerPositionDeterminer = new PlayerPositionDeterminer();
            AvailablePlayerCount = _totalMainPlayers;
            MainPlayers = _playerPositionDeterminer.GetPlayersWithPosition(TacticSchema, AllPlayers);

            foreach (var player in MainPlayers.Values)
            {
                PlayersInMatch.Add(createPlayerInMatch(player.CurrentPlayer.ContractID));
            }
        }

        private PlayerInMatch createPlayerInMatch(string playerId, int startMinute = 0)
        {
            return new PlayerInMatch()
            {
                PlayerId = playerId,
                TeamId = Id,
                StartMinute = startMinute,
                LastMinute = 90,
            };
        }

        //Implemented SubstitutePlayer method
        public void SubstitutePlayer(int indexMainPlayer, Player sparePlayer,int currentMinute)
        {
            if(MainPlayers.TryGetValue(indexMainPlayer,out var playerPosition))
            {
                SparedPlayers.Add(playerPosition.CurrentPlayer);
                playerPosition.CurrentPlayer = sparePlayer;

                foreach (var playerInMatch in PlayersInMatch)
                {
                    if (playerInMatch.PlayerId == playerPosition.CurrentPlayer.PersonID)
                    {
                        playerInMatch.LastMinute = currentMinute;
                    }
                }
                PlayersInMatch.Add(createPlayerInMatch(sparePlayer.PersonID, currentMinute));
            }
            else
            {
                throw new System.Exception("Index doesn`t belong to that team");
            }
        }
        public void ChangeTacticScheme(TacticSchema newTacticSchema)
        {
            TacticSchema = newTacticSchema;
            MainPlayers = _playerPositionDeterminer.GetPlayersWithPosition(newTacticSchema,
            MainPlayers.Values.Select(item => item.CurrentPlayer).ToList());
        }
        public Player GetPlayer(PlayerFieldPartPosition playerPostion)
        {
            var player = getPlayer(playerPostion);
            return player != null ? player : MainPlayers.Where(player => player.Value.CurrentPlayer != null).FirstOrDefault().Value.CurrentPlayer;
        }

        private Player getPlayer(PlayerFieldPartPosition playerPostion)
        {
            var selectedPlayers = MainPlayers.Where(player => player.Value.CurrentPlayer != null && player.Value.FieldPosition == playerPostion);
            if (selectedPlayers.Count() > 0)
            {
                var players = MainPlayers.Where(player => player.Value.CurrentPlayer != null).Select(x => x.Value.CurrentPlayer);
                var random = new System.Random();
                return players.ElementAt(random.Next(0, players.Count() - 1));
            }
            return null;
        }
        public Player GetPlayer(double attackProb, double midProb, double defProb, double keeperProb)
        {
            var totalValue = attackProb + midProb + defProb + keeperProb;
            var random = new Random();
            var resultValue = random.NextDouble() * totalValue;
            Player player = new Player();
            if(resultValue >= 0 && resultValue <= attackProb)
            {
                player = getPlayer(PlayerFieldPartPosition.Attack);
                return player != null ? player : GetPlayer(0, midProb, defProb, keeperProb);
            }
            resultValue -= attackProb;
            if (resultValue >= 0 && resultValue <= midProb)
            {
                player = getPlayer(PlayerFieldPartPosition.Midfield);
                return player != null ? player : GetPlayer(attackProb, 0, defProb, keeperProb);
            }
            resultValue -= midProb;
            if (resultValue >= 0 && resultValue <= defProb)
            {
                player = getPlayer(PlayerFieldPartPosition.Defence);
                return player != null ? player : GetPlayer(attackProb, midProb, 0, keeperProb);
            }
            resultValue -= defProb;
            if (resultValue >= 0 && resultValue <= keeperProb)
            {
                player = getPlayer(PlayerFieldPartPosition.Goalkeeper);
                return player != null ? player : GetPlayer(attackProb, midProb, midProb, 0);
            }
            return null;
        }
        public double AvgSpeed(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            var result  = (double)MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Speed);
            
            var playersCountByPosition = 0;
            var totalSpeed = 0;
            foreach(var player in MainPlayers)
            {
                if (player.Value.FieldPosition == playerPostion)
                {
                    playersCountByPosition++;
                    if(player.Value.CurrentPlayer != null)
                    {
                        totalSpeed += player.Value.CurrentPlayer.Speed;
                    }
                }
            }
            
            if(playersCountByPosition > 0 && totalSpeed > 0)
            {
                result = (double)totalSpeed/(double)playersCountByPosition;
            }
            return result * ((double)AvailablePlayerCount/(double)_totalMainPlayers);
        }
        public double AvgStrike(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            var result = (double)MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Strike);

            var playersCountByPosition = 0;
            var totalStrike = 0;
            foreach (var player in MainPlayers)
            {
                if (player.Value.FieldPosition == playerPostion)
                {
                    playersCountByPosition++;
                    if (player.Value.CurrentPlayer != null)
                    {
                        totalStrike += player.Value.CurrentPlayer.Strike;
                    }
                }
            }

            if (playersCountByPosition > 0 && totalStrike > 0)
            {
                result = (double)totalStrike / (double)playersCountByPosition;
            }
            return result * ((double)AvailablePlayerCount / (double)_totalMainPlayers);
        }
        public double AvgDefense(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            var result = (double)MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Defending);

            var playersCountByPosition = 0;
            var totalDefending = 0;
            foreach (var player in MainPlayers)
            {
                if (player.Value.FieldPosition == playerPostion)
                {
                    playersCountByPosition++;
                    if (player.Value.CurrentPlayer != null)
                    {
                        totalDefending += player.Value.CurrentPlayer.Defending;
                    }
                }
            }
            if (playersCountByPosition > 0 && totalDefending > 0)
            {
                result = (double)totalDefending / (double)playersCountByPosition;
            }
            return result * ((double)AvailablePlayerCount / (double)_totalMainPlayers);
        }
        public double AvgPhysicalTraining(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            var result = (double)MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Physics);

            var playersCountByPosition = 0;
            var totalPhysics = 0;
            foreach (var player in MainPlayers)
            {
                if (player.Value.FieldPosition == playerPostion)
                {
                    playersCountByPosition++;
                    if (player.Value.CurrentPlayer != null)
                    {
                        totalPhysics += player.Value.CurrentPlayer.Physics;
                    }
                }
            }
            if (playersCountByPosition > 0 && totalPhysics > 0)
            {
                result = (double)totalPhysics / (double)playersCountByPosition;
            }
            return result * ((double)AvailablePlayerCount / (double)_totalMainPlayers);
        }

        public double AvgTechnique(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            var result = (double)MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Dribbling);

            var playersCountByPosition = 0;
            var totalDribbling = 0;
            foreach (var player in MainPlayers)
            {
                if (player.Value.FieldPosition == playerPostion)
                {
                    playersCountByPosition++;
                    if (player.Value.CurrentPlayer != null)
                    {
                        totalDribbling += player.Value.CurrentPlayer.Dribbling;
                    }
                }
            }
            if (playersCountByPosition > 0 && totalDribbling > 0)
            {
                result = (double)totalDribbling / (double)playersCountByPosition;
            }
            return result * ((double)AvailablePlayerCount / (double)_totalMainPlayers);
        }

        public double AvgPassing(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All)
        {
            var result = (double)MainPlayers.Where(player => player.Value.CurrentPlayer != null).
                    Average(p => p.Value.CurrentPlayer.Passing);

            var playersCountByPosition = 0;
            var totalPassing = 0;
            foreach (var player in MainPlayers)
            {
                if (player.Value.FieldPosition == playerPostion)
                {
                    playersCountByPosition++;
                    if (player.Value.CurrentPlayer != null)
                    {
                        totalPassing += player.Value.CurrentPlayer.Passing;
                    }
                }
            }
            if (playersCountByPosition > 0 && totalPassing > 0)
            {
                result = (double)totalPassing / (double)playersCountByPosition;
            }
            return result * ((double)AvailablePlayerCount / (double)_totalMainPlayers);
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
