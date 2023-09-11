using BusinessLogicLayer.Rules;
using DatabaseLayer;
using DatabaseLayer.Enums;
using DatabaseLayer.Repositories;
using DatabaseLayer.Services;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class CompabilityPlayerPositionChecker
    {
        PlayerRepository _playerRepository;
        PlayerFieldPartPositionConvertor _playerFieldPartPositionConvertor;
        NewPositionCreator _newPositionCreator;
        TacticSchemaFactory _tacticSchemaFactory;
        TeamRepository _teamRepository;
        ContractRepository _contractRepository;
        public CompabilityPlayerPositionChecker()
        {
            _playerRepository = new PlayerRepository();
            _playerFieldPartPositionConvertor = new PlayerFieldPartPositionConvertor();
            _newPositionCreator = new NewPositionCreator();
            _tacticSchemaFactory = new TacticSchemaFactory();
            _teamRepository = new TeamRepository();
            _contractRepository = new ContractRepository();
        }
        public TacticPlayerPosition Check(PlayerPosition playerPosition, string playerId)
        {
            var player = _playerRepository.RetrieveOne(playerId);

            if(player != null)
            {
                var fieldPartPosition = _playerFieldPartPositionConvertor.Convert(playerPosition);
                var checkedPositionCode = EnumDescription.GetEnumDescription(playerPosition);

                if (checkedPositionCode == player.Position.Code)
                {
                    return new TacticPlayerPosition()
                    {
                        CurrentPlayer = player,
                        RealPosition = checkedPositionCode,
                        FieldPosition = fieldPartPosition,
                        IndexPosition = definePositionIndexByContractId(player.Position.Code, player.ContractID)
                    };
                }
                var samePositions = TacticSchemeNavigation.GetSamePosition(playerPosition);
                var tacticPlayerPos = getTacticPlayerPositionOnAnotherPosition(player, samePositions, checkedPositionCode, fieldPartPosition);
                
                return tacticPlayerPos;
            }

            return new TacticPlayerPosition();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerPosition"></param>
        /// <param name="playerIds"></param>
        /// <returns>Returns a dictionary where PersonID is the key and TacticPlayerPosition is the value by the given position</returns>
        public Dictionary<string, TacticPlayerPosition> Check(PlayerPosition playerPosition, List<string> playerIds)
        {
            var result = new Dictionary<string, TacticPlayerPosition>();

            var fieldPartPosition = _playerFieldPartPositionConvertor.Convert(playerPosition);
            var checkedPositionCode = EnumDescription.GetEnumDescription(playerPosition);

            var players = _playerRepository.Retrieve(playerIds).Values.ToList();
            var playersOnAnotherPosition = new List<Player>();

            foreach(var player in players)
            {
                if (player != null)
                {
                    if (checkedPositionCode != player.Position.Code)
                    {
                        playersOnAnotherPosition.Add(player);
                    }
                    else
                    {
                        var tacticPlayerPos = new TacticPlayerPosition()
                        {
                            CurrentPlayer = player,
                            RealPosition = player.Position.Code,
                            FieldPosition = fieldPartPosition,
                            IndexPosition = definePositionIndexByContractId(player.Position.Code, player.ContractID)
                        };
                        result.Add(player.PersonID, tacticPlayerPos);
                    }
                }
            }
            if(playersOnAnotherPosition.Count > 0)
            {
                var samePositions = TacticSchemeNavigation.GetSamePosition(playerPosition);
                foreach(var player in playersOnAnotherPosition)
                {
                    var tacticPlayerPos = getTacticPlayerPositionOnAnotherPosition(player, samePositions, checkedPositionCode, fieldPartPosition);
                    result.Add(player.PersonID, tacticPlayerPos);
                }
            }
            return result;
        }

        private TacticPlayerPosition getTacticPlayerPositionOnAnotherPosition(Player player, Dictionary<string, int> samePositions, string checkedPositionCode, PlayerFieldPartPosition fieldPartPosition)
        {
            var position = _newPositionCreator.Create(checkedPositionCode, fieldPartPosition);
            
            player.Position = position;
            player.PositionCode = checkedPositionCode;

            if (samePositions.TryGetValue(player.PositionCode, out int rating))
            {
                player.UpdateCurrentRating(rating);
            }
            else if (samePositions.TryGetValue("", out rating))
            {
                player.UpdateCurrentRating(rating);
            }

            return new TacticPlayerPosition()
            {
                CurrentPlayer = player,
                RealPosition = checkedPositionCode,
                FieldPosition = fieldPartPosition,
                IndexPosition = definePositionIndexByContractId(player.Position.Code, player.ContractID)
            };
        }

        private int definePositionIndexByContractId(string positionCode, string contractId)
        {
            var playerContract = _contractRepository.RetrieveOne(contractId);
            var teamId = playerContract.TeamId;
            var positionIndexes = getPositionsIndexes(teamId);
            return defineIndex(positionIndexes, positionCode);
        }

        private Dictionary<int, string> getPositionsIndexes(string teamId)
        {
            var team = _teamRepository.Retrieve(teamId);
            return _tacticSchemaFactory.GetPlayersPosition(team.TacticSchema);
        }

        private int defineIndex(Dictionary<int, string> positionsIndexes, string positionCode)
        {
            foreach (var position in positionsIndexes)
            {
                if (position.Value == positionCode)
                {
                    return position.Key;
                }
            }
            return 0;
        }
    }
}
