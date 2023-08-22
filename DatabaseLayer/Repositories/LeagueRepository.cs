using Dapper;
using DatabaseLayer.DBSettings;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using DatabaseLayer.Enums;

namespace DatabaseLayer.Repositories
{
    public class LeagueRepository
    {
        public List<League> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var sql = @"SELECT League.*, Country.*
                    FROM League 
                    INNER JOIN Country ON League.CountryID = Country.ID
                    WHERE League.RowState = @rowState";
                var results = connection.Query<League, Country, League>(
                    sql,
                    (league, country) =>
                    {
                        league.Country = country;
                        return league;
                    },
                    param: new { rowState = DbRowState.IsActive },
                    splitOn: "Id"
                );
                return results.AsList();
            }
        }
        public League Retrieve(long leagueId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var sql = @"SELECT l.*, c.*
                    FROM League l
                    INNER JOIN Country c ON l.CountryID = c.ID
                    WHERE l.ID = @leagueId";
                var results = connection.Query<League, Country, League>(
                    sql,
                    (league, country) =>
                    {
                        league.Country = country;
                        return league;
                    },
                    param: new { leagueId },
                    splitOn: "Id"
                );
                return results.FirstOrDefault();
            }
        }
        public int RetrieveLeagueIdByTeamId(string teamId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                int? leagueId = connection.QuerySingleOrDefault<int>("SELECT LeagueID FROM Team WHERE ID = @teamId", new { teamId });
                return (int)(leagueId != null ? leagueId : 0);
            }
        }
        public bool IsTeamInLeague(string teamId,List<int> leagueIds)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var response = connection.QuerySingleOrDefault<int>(@"
                SELECT 1 FROM Team 
                WHERE LeagueId IN @LeagueIds AND ID = @teamId;", new { leagueIds,teamId });
                return response == 1;
            }
        }
        internal bool Insert(League league)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<League>("SELECT * FROM League WHERE ID = @leagueId", new { leagueId = league.Id });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute("INSERT INTO League (ID, Name, CurrentRating, CountryId, ExtId) VALUES (@ID, @Name, @CurrentRating, @CountryId, @ExtId)",
                        league);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        public bool Update(League league)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<League>("SELECT * FROM League WHERE ID = @leagueId", new { leagueId = league.Id });
                bool result = false;
                if (record != null)
                {
                    var rowsAffected = connection.Execute("UPDATE League SET Name = @Name, CurrentRating = @CurrentRating, CountryID = @CountryId, ExtId = @ExtId WHERE ID = @Id",
                        league);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        public bool Delete(int leagueId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute("DELETE FROM League  WHERE Id = @leagueId ",
                    new { leagueId });
                return rowsAffected == 1;
            }
        }
    }
}

