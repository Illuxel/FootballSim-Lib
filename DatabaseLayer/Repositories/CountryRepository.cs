﻿using System.Collections.Generic;
using DatabaseLayer.DBSettings;
using System.Data.SQLite;
using Dapper;

namespace FootBalLife.Database.Repositories
{
    public class CountryRepository
    {
        public List<Country> Retrive()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                return connection.Query<Country>("SELECT * FROM Country").AsList();
            }
        }
        public Country Retrive(int countryId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Country>("SELECT * FROM Country WHERE ID = @countryId", new { countryId });
            }
        }

        internal bool Insert(Country country)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Country>("SELECT * FROM Country WHERE ID = @countryId", new { countryId = country.Id });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute("INSERT INTO Country (ID, Icon, Name) VALUES (@Id, @Icon, @Name)",
                        country);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        internal bool Update(Country country)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Country>("SELECT * FROM Country WHERE ID = @countryId", new { countryId = country.Id });
                bool result = false;
                if (record != null)
                {
                    var rowsAffected = connection.Execute("UPDATE Country SET Icon = @Icon, Name = @Name WHERE ID = @Id",
                        country);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        public bool Delete(int countryId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute("DELETE FROM Country WHERE ID = @countryId ",
                    new { countryId });
                return rowsAffected == 1;
            }
        }
    }
}
