using System.Collections.Generic;
using DatabaseLayer.DBSettings;
using System.Data.SQLite;
using Dapper;
using System.Linq;

namespace FootBalLife.Database.Repositories
{
    public class PersonRepository
    {
        public List<Person> Retrive()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var query = @"SELECT Person.*, Role.*, Country.*
                    FROM Person 
                    LEFT JOIN Role ON Person.CurrentRoleID = Role.ID
                    LEFT JOIN Country on Country.ID = Person.CountryID";
                var leagues = connection.Query<Person, Role, Country, Person>(query,
                    (person, role, country) =>
                    {
                        person.CurrentRole = role;
                        person.Country = country;
                        return person;
                    },
                    splitOn: "TeamID, PersonID");

                return leagues.AsList();
            }
        }
        public Person Retrive(string personId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var query = @"SELECT Person.*, Role.*, Country.*
                    FROM Person 
                    LEFT JOIN Role t ON Person.CurrentRoleID = Role.ID
                    LEFT JOIN Country on Country.ID = Person.CountryID
                    WHERE Person.ID = @personId";
                var leagues = connection.Query<Person, Role, Country, Person>(query,
                    (person, role, country) =>
                    {
                        person.CurrentRole = role;
                        person.Country = country;
                        return person;
                    },
                    param: new { personId },
                    splitOn: "TeamID, PersonID");

                return leagues.FirstOrDefault();
            }
        }

        public bool Insert(Person person)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Agent>("SELECT * FROM Person WHERE ID = @personID", new { personID = person.Id });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute(
                        @"INSERT INTO Person (ID, Name, Surname, Birthday, CurrentRoleId, CountryId, Icon)
                        VALUES (@ID, @Name, @Surname, @Birthday, @CurrentRoleId, @CountryId, @Icon)",
                        person);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        public bool Update(Person person)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Agent>("SELECT * FROM Person WHERE ID = @personID", new { personID = person.Id });
                bool result = false;
                if (record != null)
                {
                    var rowsAffected = connection.Execute(
                        @"UPDATE Person 
                           SET Name = @Name, 
                               Surname = @Surname, 
                               Birthday = @Birthday, 
                               CurrentRoleId = @CurrentRoleId, 
                               CountryId = @CountryId, 
                               Icon = @Icon 
                           WHERE ID = @ID",
                        person);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }
        public bool Delete(string personId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute("DELETE FROM Person WHERE Id = @personID ",
                    new { personId });
                return rowsAffected == 1;
            }
        }
    }
}

