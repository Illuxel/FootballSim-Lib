using DatabaseLayer;
using DatabaseLayer.Repositories;
using System.Collections.Generic;
using System.Drawing;

namespace BusinessLogicLayer.Services
{
    public class GoalAssistTracker
    {
        GoalRepository _goalRepepository;
        SeasonValueCreator _seasonValueCreator;
        public GoalAssistTracker()
        {
            _goalRepepository = new GoalRepository();
            _seasonValueCreator = new SeasonValueCreator();
        }

        public List<PlayerStatistic> GetTopGoalScorers(string season,int leagueId,int count = 10)
        {
            var seasonStartDate = _seasonValueCreator.GetSeasonStartDate(season);
            var seasonEndDate = _seasonValueCreator.GetSeasonEndDate(season);
            var topBambardiers = _goalRepepository.GetTopGoalScorers(leagueId, season, seasonStartDate, seasonEndDate,count);

            return topBambardiers;
        }

        public List<PlayerStatistic> GetTopAssists(string season, int leagueId, int count = 10)
        {
            var seasonStartDate = _seasonValueCreator.GetSeasonStartDate(season);
            var seasonEndDate = _seasonValueCreator.GetSeasonEndDate(season);
            var topAssists = _goalRepepository.GetTopAssistents(leagueId, season, seasonStartDate, seasonEndDate,count);

            return topAssists;
        }

        public List<PlayerStatistic> GetPlayerStatistic(string playerId)
        {
            return _goalRepepository.GetPlayerStatistic(playerId);
        }

        public List<PlayerStatistic> GetPlayerStatistic(string playerId, string season)
        {
            var seasonStartDate = _seasonValueCreator.GetSeasonStartDate(season);
            var seasonEndDate = _seasonValueCreator.GetSeasonEndDate(season);
            return new List<PlayerStatistic>();
        }
    }
}
