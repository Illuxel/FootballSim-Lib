using System.Collections.Generic;
using DatabaseLayer.DBSettings;
using System.Data.SQLite;
using Dapper;

namespace FootBalLife.Database.Repositories
{
    public class PersonNameRepository
    {
        public List<PersonName> RetriveNames()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                return connection.Query<PersonName>("SELECT * FROM PersonName").AsList();
            }
        }

        public List<PersonSurname> RetriveSurnames()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                return connection.Query<PersonSurname>("SELECT * FROM PersonSurname").AsList();
            }
        }
        public List<PersonName> RetriveNames(int countryId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                return connection.Query<PersonName>("SELECT * FROM PersonName WHERE CountryId = @countryId", new { countryId }).AsList();
            }
        }

        public List<PersonSurname> RetriveSurnames(int countryId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                return connection.Query<PersonSurname>("SELECT * FROM PersonSurname WHERE CountryId = @countryId", new { countryId }).AsList();
            }
        }
    }
}

