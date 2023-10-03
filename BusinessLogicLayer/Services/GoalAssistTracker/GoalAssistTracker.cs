using DatabaseLayer;
using DatabaseLayer.Repositories;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    public class GoalAssistTracker
    {
        GoalRepository _goalRepository;
        SeasonValueCreator _seasonValueCreator;
        public GoalAssistTracker()
        {
            _goalRepository = new GoalRepository();
            _seasonValueCreator = new SeasonValueCreator();
        }

        public List<PlayerStatistic> GetTopGoalScorers(string season,int leagueId,int count = 10)
        {
            var seasonStartDate = _seasonValueCreator.GetSeasonStartDate(season);
            var seasonEndDate = _seasonValueCreator.GetSeasonEndDate(season);
            var topBambardiers = _goalRepository.GetTopGoalScorers(leagueId, season, seasonStartDate, seasonEndDate,count);

            return topBambardiers;
        }

        public List<PlayerStatistic> GetTopAssists(string season, int leagueId, int count = 10)
        {
            var seasonStartDate = _seasonValueCreator.GetSeasonStartDate(season);
            var seasonEndDate = _seasonValueCreator.GetSeasonEndDate(season);
            var topAssists = _goalRepository.GetTopAssistents(leagueId, season, seasonStartDate, seasonEndDate,count);

            return topAssists;
        }

        public List<PlayerStatistic> GetPlayerStatistic(string playerId)
        {
            return _goalRepository.GetPlayerStatistic(playerId);
        }
    }
}
