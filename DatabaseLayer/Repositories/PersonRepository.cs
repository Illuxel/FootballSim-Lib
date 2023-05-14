using System.Collections.Generic;
using DatabaseLayer.DBSettings;
using System.Data.SQLite;
using Dapper;
using System.Linq;
using System;
using System.Data;

namespace DatabaseLayer.Repositories
{
    public class PersonRepository
    {
        public List<Person> Retrieve()
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
        public Person Retrieve(string personId)
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
                person.Id = Guid.NewGuid().ToString();
               
                var rowsAffected = connection.Execute(
                    @"INSERT INTO Person (ID, Name, Surname, Birthday, CurrentRoleId, CountryId, Icon)
                    VALUES (@ID, @Name, @Surname, @Birthday, @CurrentRoleId, @CountryId, @Icon)",
                    person);
                var result = rowsAffected == 1;
                
                return result;
            }
        }

        public bool Insert(List<Person> persons)
        {
            foreach(var person in persons)
            {
                person.Id = Guid.NewGuid().ToString();
            }

            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var rowsAffected = connection.Execute(
                        @"INSERT INTO Person (ID, Name, Surname, Birthday, CurrentRoleId, CountryId, Icon)
                            VALUES (@ID, @Name, @Surname, @Birthday, @CurrentRoleId, @CountryId, @Icon)",
                        persons, transaction);
                        return rowsAffected == 1;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
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

