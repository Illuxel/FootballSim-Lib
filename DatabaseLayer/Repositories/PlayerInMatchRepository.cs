using System;
using System.Data.SQLite;
using System.Collections.Generic;

using Dapper;

using DatabaseLayer.Settings;

namespace DatabaseLayer.Repositories
{
    public class PlayerInMatchRepository
    {
        public List<PlayerInMatch> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var response = connection.Query<PlayerInMatch>("SELECT * FROM PlayerInMatch").AsList();
                return response;
            }
        }
        
        public List<PlayerInMatch> Retrieve(string playerId)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var response = connection.Query<PlayerInMatch>("SELECT * FROM PlayerInMatch WHERE PlayerId = @playerId", new { playerId}).AsList();
                return response;
            }
        }

        public List<PlayerInMatch> RetrieveByTeam(string teamId)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var response = connection.Query<PlayerInMatch>("SELECT * FROM PlayerInMatch WHERE TeamId = @teamId", new { teamId }).AsList();
                return response;
            }
        }

        //key - teamId, Value - players
        public Dictionary<string, List<string>> RetrieveByLastMatches()
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var dynamicsResponse  = connection.Query(@"
                    SELECT t.ID AS TeamID, pim.PlayerId AS PlayerID
                    FROM (
                        SELECT m.LeagueId, MAX(m.TourNumber) AS MaxTourNumber
                        FROM Match m
                        WHERE m.IsPlayed = 1
                        GROUP BY m.LeagueId
                    ) last_matches
                    JOIN Match m ON m.LeagueId = last_matches.LeagueId AND m.TourNumber = last_matches.MaxTourNumber
                    JOIN Team t ON t.ID = m.HomeTeamId OR t.ID = m.GuestTeamId
                    JOIN PlayerInMatch pim ON pim.MatchId = m.Id AND t.ID = pim.TeamId
                    ORDER BY t.ID");

                var result = new Dictionary<string, List<string>>();
                foreach (var row in dynamicsResponse)
                {
                    if (!result.TryGetValue(row.TeamID, out List<string> players))
                    {
                        players = new List<string>();
                        result[row.TeamID] = players;
                    }
                    players.Add(row.PlayerID);
                }
                return result;
            }
        }

        public bool Insert(PlayerInMatch player)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
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
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
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
