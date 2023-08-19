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
                G.PlayerId,
                COUNT(G.PlayerId) as GoalsCount,
                (
                    SELECT COUNT(*)
                    FROM PlayerInMatch PIM
                    WHERE PIM.PlayerId = G.PlayerId
                ) AS TotalMatch
                FROM
                    Goal G
                JOIN
                    ""Match"" M ON G.MatchId = M.Id
                WHERE
                    M.LeagueId = @leagueId AND M.MatchDate BETWEEN @seasonStartDate AND @seasonEndDate AND G.PlayerId NOT IN ('00000000-0000-0000-0000-000000000000', '')
                GROUP BY
                    G.PlayerId
                ORDER BY
                    GoalsCount DESC,
                    TotalMatch ASC
                LIMIT @limit;", 
                new { LeagueId = leagueId, SeasonStartDate = seasonStartDate, SeasonEndDate = seasonEndDate, Limit = limit });

                var playerRepository = new PlayerRepository();
                var players = new List<PlayerStatistic>();

                foreach (var player in result)
                {
                    var playerStatistic = new PlayerStatistic
                    {
                        Player = playerRepository.RetrieveOne(player.PlayerId),
                        CountOfGoals = (int)player.GoalsCount,
                        CountOfPlayedMatches = (int)player.TotalMatch,
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
                G.AssistPlayerId,
                COUNT(G.AssistPlayerId) as AssistCount,
                (
                    SELECT COUNT(*)
                    FROM PlayerInMatch PIM
                    WHERE PIM.PlayerId = G.AssistPlayerId
                ) AS TotalMatch
                FROM
                    Goal G
                JOIN
                    ""Match"" M ON G.MatchId = M.Id
                WHERE
                    M.LeagueId = @leagueId AND M.MatchDate BETWEEN @seasonStartDate AND @seasonEndDate AND AssistPlayerId NOT IN ('00000000-0000-0000-0000-000000000000', '')
                GROUP BY
                    G.AssistPlayerId
                ORDER BY
                    AssistCount DESC,
                    TotalMatch ASC
                LIMIT @limit;", 
                new { LeagueId = leagueId, SeasonStartDate = seasonStartDate, SeasonEndDate = seasonEndDate, Limit = limit });

                var playerRepository = new PlayerRepository();
                var players = new List<PlayerStatistic>();

                foreach (var player in result)
                {
                    var playerStatistic = new PlayerStatistic
                    {
                        Player = playerRepository.RetrieveOne(player.PlayerId),
                        CountOfAssists = (int)player.AssistCount,
                        CountOfPlayedMatches = (int)player.TotalMatch,
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
