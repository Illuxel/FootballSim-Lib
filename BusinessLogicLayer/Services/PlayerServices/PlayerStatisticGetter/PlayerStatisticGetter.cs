﻿using DatabaseLayer.Repositories;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class PlayerStatisticGetter
    {
        SeasonValueCreator _seasonValueCreator;
        ContractRepository _contractRepository;
        MatchRepository _matchRepository;
        PlayerInMatchRepository _playerInMatchRepository;

        public PlayerStatisticGetter()
        {
            _seasonValueCreator = new SeasonValueCreator();
            _contractRepository = new ContractRepository();
            _matchRepository = new MatchRepository();
            _playerInMatchRepository = new PlayerInMatchRepository();
        }

        public PlayerInvolvement GetPlayedMatch(string playerId, string season)
        {

            var teamId = getTeamId(playerId);

            var allMatchesCount = getAllMatchesCount(teamId, season);

            var playerInMatchesCount = getPlayerInMatchesCount(playerId);

            return new PlayerInvolvement()
            {
                TotalMatch = allMatchesCount,
                PlayedMatch = playerInMatchesCount
            };
        }

        private string getTeamId(string playerId)
        {
            if (string.IsNullOrEmpty(playerId))
            {
                return string.Empty;
            }
            else
            {
                return _contractRepository.Retrieve(playerId).First().TeamId;
            }
        }
        private int getAllMatchesCount(string teamId, string season)
        {
            if (string.IsNullOrEmpty(teamId) || string.IsNullOrEmpty(season))
            {
                return 0;
            }

            var startDate = _seasonValueCreator.GetSeasonStartDate(season);
            var endDate = _seasonValueCreator.GetSeasonEndDate(season);

            return _matchRepository.Retrieve(teamId, startDate, endDate).Count;
        }
        private int getPlayerInMatchesCount(string playerId)
        {
            if (string.IsNullOrEmpty(playerId))
            {
                return 0;
            }

            return _playerInMatchRepository.Retrieve(playerId).Count;
        }
    }
}
