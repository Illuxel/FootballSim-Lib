using BusinessLogicLayer.Rules;
using DatabaseLayer;
using DatabaseLayer.Model;
using DatabaseLayer.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class PlayerPositionDeterminer
    {
        private TeamRepository _teamRepository;
        private TacticSchemaFactory _tacticSchemaFactory;
        private PlayerFieldPartPositionConvertor _playerFieldPartPositionConvertor;
        public PlayerPositionDeterminer()
        {
            _teamRepository = new TeamRepository();
            _tacticSchemaFactory = new TacticSchemaFactory();
            _playerFieldPartPositionConvertor = new PlayerFieldPartPositionConvertor();
        }

        public Dictionary<int, TacticPlayerPosition> GetPlayersWithPosition(TacticSchema tacticSchema, List<Player> players)
        {
            var playersPositions = _tacticSchemaFactory.GetPlayersPosition(tacticSchema);
            var teamTactic = SetPlayers(players, playersPositions);
            return teamTactic;
        }

        public Dictionary<int, TacticPlayerPosition> GetPlayersWithPosition(TacticSchema tacticSchema, string teamId)
        {
            var team = _teamRepository.Retrieve(teamId);
            if (team != null)
            {
                var teamTactic = GetPlayersWithPosition(tacticSchema, team.Players);
                return teamTactic;
            }
            return new Dictionary<int, TacticPlayerPosition>();
        }

        internal Dictionary<int, TacticPlayerPosition> SetPlayers(List<Player> players, Dictionary<int, string> positions)
        {
            var playersWithPositions = new Dictionary<int, TacticPlayerPosition>();
            var existsPlayers = new HashSet<string>();

            //try to find native players position
            foreach (var position in positions)
            {
                var playerSamePositions = new List<Player>();
                foreach (var player in players.Where(pl => !existsPlayers.Contains(pl.PersonID)))
                {
                    if (position.Value == player.PositionCode)
                    {
                        playerSamePositions.Add(player);
                    }
                }
                if (playerSamePositions.Count > 0)
                {
                    var selectedPlayer = playerSamePositions.OrderByDescending(item => item.Rating).FirstOrDefault();
                    playersWithPositions[position.Key] = new TacticPlayerPosition()
                    {
                        CurrentPlayer = selectedPlayer,
                        IndexPosition = position.Key,
                        RealPosition = position.Value,
                        FieldPosition = _playerFieldPartPositionConvertor.Convert(position.Value)
                    };
                    existsPlayers.Add(selectedPlayer.PersonID);
                }
            }

            if (playersWithPositions.Count == 11)
            {
                return playersWithPositions;
            }


            var freePlayers = players.Except(playersWithPositions.Select(item => item.Value.CurrentPlayer));
            if (freePlayers.Count() == 0)
            {
                return playersWithPositions;
            }
            //try to find same players position
            var emptyPositionsIndex = positions.Keys.Except(playersWithPositions.Keys);
            foreach (var emptyPosition in emptyPositionsIndex)
            {
                var positionCode = positions[emptyPosition];
                var samePositions = TacticSchemeNavigation.GetSamePosition(positionCode);
                freePlayers = players.Except(playersWithPositions.Select(item => item.Value.CurrentPlayer));
                if (freePlayers.Count() == 0)
                {
                    return playersWithPositions;
                }
                var selectedPlayers = new List<Player>();
                foreach (var freePlayer in freePlayers)
                {
                    if (samePositions.TryGetValue(freePlayer.PositionCode, out int changedValue))
                    {
                        freePlayer.UpdateCurrentRating(changedValue);
                        selectedPlayers.Add(freePlayer);
                    }
                }

                if (selectedPlayers.Count > 0)
                {
                    var selectedPlayer = selectedPlayers.OrderByDescending(item => item.CurrentPlayerRating).FirstOrDefault();
                    playersWithPositions[emptyPosition] = new TacticPlayerPosition()
                    {
                        CurrentPlayer = selectedPlayer,
                        IndexPosition = emptyPosition,
                        RealPosition = positionCode,
                        FieldPosition = _playerFieldPartPositionConvertor.Convert(positionCode)
                    };
                    existsPlayers.Add(selectedPlayer.PersonID);
                }


                if (playersWithPositions.Count == 11)
                {
                    return playersWithPositions;
                }
            }

            //try to find any players
            var stillEmptyPositionsIndex = positions.Keys.Except(playersWithPositions.Keys);
            foreach (var stillEmptyPos in stillEmptyPositionsIndex)
            {
                freePlayers = players.Except(playersWithPositions.Select(item => item.Value.CurrentPlayer));
                if (freePlayers.Count() == 0)
                {
                    return playersWithPositions;
                }

                var selectedPlayer = freePlayers.OrderBy(item => item.CurrentPlayerRating).FirstOrDefault();
                selectedPlayer.UpdateCurrentRating(-20);
                var positionCode = positions[stillEmptyPos];
                playersWithPositions[stillEmptyPos] = new TacticPlayerPosition()
                {
                    IndexPosition = stillEmptyPos,
                    CurrentPlayer = selectedPlayer,
                    RealPosition = positionCode,
                    FieldPosition = _playerFieldPartPositionConvertor.Convert(positionCode)
                };

                if (playersWithPositions.Count == 11)
                {
                    return playersWithPositions;
                }
            }
            return playersWithPositions;
        }

    }
}
