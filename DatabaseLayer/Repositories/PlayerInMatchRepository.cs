using Dapper;
using DatabaseLayer.DBSettings;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace DatabaseLayer.Repositories
{
    public class PlayerInMatchRepository
    {
        public List<PlayerInMatch> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var response = connection.Query<PlayerInMatch>("SELECT * FROM PlayerInMatch").AsList();
                return response;
            }
        }
        
        public List<PlayerInMatch> Retrieve(string playerId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var response = connection.Query<PlayerInMatch>("SELECT * FROM PlayerInMatch WHERE PlayerId = @playerId", new { playerId}).AsList();
                return response;
            }
        }

        public List<PlayerInMatch> RetrieveByTeam(string teamId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var response = connection.Query<PlayerInMatch>("SELECT * FROM PlayerInMatch WHERE TeamId = @teamId", new { teamId }).AsList();
                return response;
            }
        }

        public bool Insert(PlayerInMatch player)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute(
                        @"INSERT INTO PlayerInMatch (Id, MatchId, PlayerId, TeamId, StartMinute, LastMinute) 
                        VALUES (@Id, @MatchId, @PlayerId, @TeamId, @StartMinute, @LastMinute)",player);

                return rowsAffected != 0;
            }
        }
        public bool Insert(List<PlayerInMatch> players)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute(
                            @"INSERT INTO PlayerInMatch (Id, MatchId, PlayerId, TeamId, StartMinute, LastMinute) 
                            VALUES (@Id, @MatchId, @PlayerId, @TeamId, @StartMinute, @LastMinute)", players);

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }
    }
}
