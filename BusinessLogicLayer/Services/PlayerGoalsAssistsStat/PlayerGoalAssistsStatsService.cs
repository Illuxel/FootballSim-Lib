using DatabaseLayer;
using DatabaseLayer.Repositories;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    public class PlayerGoalAssistsStatsService
    {
        GoalRepository _goalRepepository;
        SeasonValueCreator _seasonValueCreator;
        public PlayerGoalAssistsStatsService()
        {
            _goalRepepository = new GoalRepository();
            _seasonValueCreator = new SeasonValueCreator();
        }

        public List<PlayerStatistic> GetTopGoalScorers(string season,string leagueId,int count = 10)
        {
            var seasonStartDate = _seasonValueCreator.GetSeasonStartDate(season);
            var seasonEndDate = _seasonValueCreator.GetSeasonEndDate(season);
            var topBambardiers = _goalRepepository.GetTopGoalScorers(leagueId, season, seasonStartDate, seasonEndDate,count);

            return topBambardiers;
        }

        public List<PlayerStatistic> GetTopAssists(string season, string leagueId, int count = 10)
        {
            var seasonStartDate = _seasonValueCreator.GetSeasonStartDate(season);
            var seasonEndDate = _seasonValueCreator.GetSeasonEndDate(season);
            var topAssists = _goalRepepository.GetTopAssistents(leagueId, season, seasonStartDate, seasonEndDate,count);

            return topAssists;
        }
    }
}
