using Dapper;
using DatabaseLayer.DBSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

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

        public Dictionary<string, (int GoalsScored, int MatchesPlayed)> GetTopBombardiers(string season, string league, int limit)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var data = connection.Query(@"
                    SELECT
                        PlayerInMatch.PlayerId,
                        SUM(CASE WHEN Goal.PlayerId = PlayerInMatch.PlayerId THEN 1 ELSE 0 END) AS GoalsScored,
                        COUNT(DISTINCT PlayerInMatch.MatchId) AS MatchesPlayed
                    FROM
                        PlayerInMatch
                    JOIN
                        ""Match"" M ON PlayerInMatch.MatchId = M.Id
                    LEFT JOIN
                        Goal ON PlayerInMatch.MatchId = Goal.MatchId AND PlayerInMatch.PlayerId = Goal.PlayerId
                    WHERE
                        M.Season = @season AND M.LeagueId = @league
                    GROUP BY
                        PlayerInMatch.PlayerId
                    ORDER BY
                    GoalsScored DESC,
                    MatchesPlayed ASC
                    LIMIT @limit"
                    , new { season, league, limit })
                    .ToDictionary(
                        row => (string)row.PlayerId,
                        row => (GoalsScored: (int)row.GoalsScored, MatchesPlayed: (int)row.MatchesPlayed)
                    );

                return data;
            }
        }

        public Dictionary<string, (int Assists, int MatchesPlayed)> GetTopAssistents(string season, string league, int limit)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var data = connection.Query(@"
                    SELECT
                        PlayerInMatch.PlayerId,
                        SUM(CASE WHEN G.AssistPlayerId = PlayerInMatch.PlayerId THEN 1 ELSE 0 END) AS Assists,
                        COUNT(DISTINCT PlayerInMatch.MatchId) AS MatchesPlayed
                    FROM
                        PlayerInMatch 
                    JOIN
                        ""Match"" M ON PlayerInMatch.MatchId = M.Id
                    LEFT JOIN
                        Goal G ON PlayerInMatch.MatchId = G.MatchId AND PlayerInMatch.PlayerId = G.AssistPlayerId
                    WHERE
                        M.Season = @season AND M.LeagueId = @league
                    GROUP BY
                        PlayerInMatch.PlayerId
                    ORDER BY
                        Assists DESC,
                        MatchesPlayed ASC
                    LIMIT @limit;"
                    , new { season, league, limit })
                    .ToDictionary(
                        row => (string)row.PlayerId,
                        row => (Assists: (int)row.Assists, MatchesPlayed: (int)row.MatchesPlayed)
                    );

                return data;
            }
        }

    }
}
