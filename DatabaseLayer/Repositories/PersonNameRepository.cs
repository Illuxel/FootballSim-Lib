using System.Data.SQLite;
using System.Collections.Generic;

using Dapper;

using DatabaseLayer.Settings;

namespace DatabaseLayer.Repositories
{
    public class PersonNameRepository
    {
        public List<PersonName> RetrieveNames()
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.Query<PersonName>("SELECT * FROM PersonName").AsList();
            }
        }

        public List<PersonSurname> RetrieveSurnames()
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.Query<PersonSurname>("SELECT * FROM PersonSurname").AsList();
            }
        }
        public List<PersonName> RetrieveNames(int countryId)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.Query<PersonName>("SELECT * FROM PersonName WHERE CountryId = @countryId", new { countryId }).AsList();
            }
        }

        public List<PersonSurname> RetrieveSurnames(int countryId)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.Query<PersonSurname>("SELECT * FROM PersonSurname WHERE CountryId = @countryId", new { countryId }).AsList();
            }
        }
    }
}

