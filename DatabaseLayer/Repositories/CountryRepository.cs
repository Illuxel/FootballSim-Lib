using System;
using System.Data.SQLite;
using System.Collections.Generic;

using Dapper;

using DatabaseLayer.Settings;

namespace DatabaseLayer.Repositories
{
    public class CountryRepository
    {
        public List<Country> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.Query<Country>("SELECT * FROM Country").AsList();
            }
        }
        public Country Retrieve(int countryId)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Country>("SELECT * FROM Country WHERE ID = @countryId", new { countryId });
            }
        }

        public bool Insert(Country country)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {

                connection.Open();
                var record = connection.QuerySingleOrDefault<Country>("SELECT * FROM Country WHERE ID = @countryId", new { countryId = country.Id });
                bool result = false;
                if (record == null)
                {
                    Console.WriteLine(country.Name);
                    var rowsAffected = connection.Execute("INSERT INTO Country (ID, Icon, Name,ExtId) VALUES (@id, @icon, @name, @ExtId)",
                        country);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        internal bool Update(Country country)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Country>("SELECT * FROM Country WHERE ID = @countryId", new { countryId = country.Id });
                bool result = false;
                if (record != null)
                {
                    var rowsAffected = connection.Execute("UPDATE Country SET Icon = @Icon, Name = @Name, ExtId = @ExtId WHERE ID = @Id",
                        country);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        public bool Delete(int countryId)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute("DELETE FROM Country WHERE ID = @countryId ",
                    new { countryId });
                return rowsAffected == 1;
            }
        }
    }
}

