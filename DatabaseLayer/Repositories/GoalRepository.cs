using Dapper;
using DatabaseLayer.DBSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace DatabaseLayer.Repositories
{
    public class GoalRepository
    {
        public List<Goal> Retrieve(string matchId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var goals = connection.Query<Goal>("SELECT * FROM Goal WHERE MatchId = @matchId", matchId).AsList();

                return goals;
            }
        }

        public List<PlayerStatistic> GetTopGoalScorers(string leagueId, string season, DateTime seasonStartDate, DateTime seasonEndDate, int limit = 10)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var result = connection.Query(@"
                SELECT
                    PIM.PlayerId,
                    SUM(CASE WHEN G.PlayerId = PIM.PlayerId THEN 1 ELSE 0 END) AS CountOfGoals,
                    SUM(CASE WHEN G.AssistPlayerId = PIM.PlayerId THEN 1 ELSE 0 END) AS CountOfAssists,
                    COUNT(DISTINCT PIM.MatchId) AS CountOfPlayedMatches
                FROM
                    PlayerInMatch PIM
                JOIN
                    ""Match"" M ON PIM.MatchId = M.Id
                LEFT JOIN
                    Goal G ON PIM.MatchId = G.MatchId AND (PIM.PlayerId = G.PlayerId OR PIM.PlayerId = G.AssistPlayerId)
                WHERE
                    M.LeagueId = @LeagueId AND Date(M.MatchDate) >= Date(@SeasonStartDate) AND Date(M.MatchDate) < Date(@SeasonEndDate)
                GROUP BY
                    PIM.PlayerId
                ORDER BY
                    CountOfGoals DESC,
                    CountOfPlayedMatches ASC
                LIMIT @Limit", 
                new { LeagueId = leagueId, SeasonStartDate = seasonStartDate, SeasonEndDate = seasonEndDate, Limit = limit });

                var playerRepository = new PlayerRepository();
                var players = new List<PlayerStatistic>();

                foreach (var player in result)
                {
                    var playerStatistic = new PlayerStatistic
                    {
                        Player = playerRepository.RetrieveOne(player.PlayerId),
                        CountOfGoals = (int)player.CountOfGoals,
                        CountOfAssists = (int)player.CountOfAssists,
                        CountOfPlayedMatches = (int)player.CountOfPlayedMatches,
                        Season = season
                    };

                    players.Add(playerStatistic);
                }

                return players;
            }
        }
        public List<PlayerStatistic> GetTopAssistents(string leagueId, string season, DateTime seasonStartDate, DateTime seasonEndDate, int limit = 10)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var result = connection.Query(@"
                SELECT
                    PIM.PlayerId,
                    SUM(CASE WHEN G.PlayerId = PIM.PlayerId THEN 1 ELSE 0 END) AS CountOfGoals,
                    SUM(CASE WHEN G.AssistPlayerId = PIM.PlayerId THEN 1 ELSE 0 END) AS CountOfAssists,
                    COUNT(DISTINCT PIM.MatchId) AS CountOfPlayedMatches
                FROM
                    PlayerInMatch PIM
                JOIN
                    ""Match"" M ON PIM.MatchId = M.Id
                LEFT JOIN
                    Goal G ON PIM.MatchId = G.MatchId AND (PIM.PlayerId = G.PlayerId OR PIM.PlayerId = G.AssistPlayerId)
                WHERE
                    M.LeagueId = @LeagueId AND Date(M.MatchDate) >= Date(@SeasonStartDate) AND Date(M.MatchDate) < Date(@SeasonEndDate)
                GROUP BY
                    PIM.PlayerId
                ORDER BY
                    CountOfAssists DESC,
                    CountOfPlayedMatches ASC
                LIMIT @Limit", 
                new { LeagueId = leagueId, SeasonStartDate = seasonStartDate, SeasonEndDate = seasonEndDate, Limit = limit });

                var playerRepository = new PlayerRepository();
                var players = new List<PlayerStatistic>();

                foreach (var player in result)
                {
                    var playerStatistic = new PlayerStatistic
                    {
                        Player = playerRepository.RetrieveOne(player.PlayerId),
                        CountOfGoals = (int)player.CountOfGoals,
                        CountOfAssists = (int)player.CountOfAssists,
                        CountOfPlayedMatches = (int)player.CountOfPlayedMatches,
                        Season = season
                    };

                    players.Add(playerStatistic);
                }

                return players;
            }
        }

        public bool Insert(Goal goal)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                goal.Id = Guid.NewGuid().ToString();
                    var rowsAffected = connection.Execute(
                        @"INSERT INTO Goal (Id, MatchId, PlayerId, TeamId, MatchMinute, AssistPlayerId)
                        VALUES (@Id, @MatchId, @PlayerId, @TeamId, @MatchMinute, @AssistPlayerId)",
                        goal);
                 var result = rowsAffected == 1;
                
                return result;
            }
        }
        public bool Insert(List<Goal> goals)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var rowsAffected = connection.Execute(
                            @"INSERT INTO Goal (Id, MatchId, PlayerId, TeamId, MatchMinute, AssistPlayerId)
                        VALUES (@Id, @MatchId, @PlayerId, @TeamId, @MatchMinute, @AssistPlayerId)",
                            goals);
                        var result = rowsAffected == 1;

                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

    }
}
