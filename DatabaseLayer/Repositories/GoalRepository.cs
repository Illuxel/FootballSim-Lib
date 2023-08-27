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
