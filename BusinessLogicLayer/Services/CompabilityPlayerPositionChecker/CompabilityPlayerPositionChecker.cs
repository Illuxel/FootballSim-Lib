using BusinessLogicLayer.Rules;
using DatabaseLayer;
using DatabaseLayer.Enums;
using DatabaseLayer.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class CompabilityPlayerPositionChecker
    {
        PlayerRepository _playerRepository;
        PlayerFieldPartPositionConvertor _playerFieldPartPositionConvertor;
        public CompabilityPlayerPositionChecker()
        {
            _playerRepository = new PlayerRepository();
            _playerFieldPartPositionConvertor = new PlayerFieldPartPositionConvertor();
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
                        RealPosition = player.Position.Code,
                        FieldPosition = fieldPartPosition,
                        //Define index position
                    };
                }
                var samePositions = TacticSchemeNavigation.GetSamePosition(playerPosition);
                var tacticPlayerPos = getTacticPlayerPositionOnAnotherPosition(player, samePositions, checkedPositionCode,fieldPartPosition);
                
                return tacticPlayerPos;
            }

            return new TacticPlayerPosition();
        }

        //Key - playerId
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
                            //Define index position
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
                //?? change player position or TacticPlayerPosition RealPosition, FieldPosition is stay same or change
                CurrentPlayer = player,
                RealPosition = checkedPositionCode,
                FieldPosition = fieldPartPosition,
                //Define index position
            };
        }
    }
}
