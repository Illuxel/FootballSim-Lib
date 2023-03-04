using Dapper;
using DatabaseLayer.DBSettings;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;

namespace FootBalLife.Database.Repositories
{
    public class LeagueRepository
    {
        public List<League> Retrive()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var sql = @"SELECT League.*, Country.*
                    FROM League 
                    INNER JOIN Country ON League.CountryID = Country.ID";
                var results = connection.Query<League, Country, League>(
                    sql,
                    (league, country) =>
                    {
                        league.Country = country;
                        return league;
                    },
                    splitOn: "Id"
                );
                return results.AsList();
            }
        }
        public League Retrive(long leagueId)
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
        internal bool Insert(League league)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<League>("SELECT * FROM League WHERE ID = @leagueId", new { leagueId = league.Id });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute("INSERT INTO League (ID, Name, CurrentRating, CountryId) VALUES (@ID, @Name, @CurrentRating, @CountryId)",
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
                    var rowsAffected = connection.Execute("UPDATE League SET Name = @Name, CurrentRating = @CurrentRating, CountryID = @CountryId WHERE ID = @Id",
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

