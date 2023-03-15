using Dapper;
using DatabaseLayer.DBSettings;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;

namespace FootBalLife.Database.Repositories
{
    public class ScoutRepository
    {
        public List<Scout> Retrive()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var scouts = connection.Query<Scout>("SELECT * FROM Scout").AsList();

                var persons = connection.Query<Person>("SELECT * FROM Person WHERE ID IN @ids",
                    new { ids = scouts.Select(item => item.PersonID) }).ToDictionary(pers => pers.Id, pers => pers);
                foreach (var scout in scouts)
                {
                    if (persons.TryGetValue(scout.PersonID, out Person person))
                    {
                        scout.Person = person;
                    }
                }
                return scouts;
            }
        }
        public Scout? Retrive(string personId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var agent = connection.QueryFirstOrDefault<Scout>("SELECT * FROM Scout WHERE PersonID = @personId", new { personId });
                if (agent != null && !string.IsNullOrEmpty(agent.PersonID))
                {
                    var person = connection.QueryFirstOrDefault<Person>("SELECT * FROM Scout WHERE ID = @personId", new { personId });
                    if (person != null)
                    {
                        agent.Person = person;
                    }
                }
                return agent;
            }
        }

        public bool Modify(Scout scout)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Scout>("SELECT * FROM Scout WHERE PersonID = @personID",
                    new { personID = scout.PersonID });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute("INSERT INTO Scout (PersonID) VALUES (@personID)",
                        new { personID = scout.PersonID });
                    result = rowsAffected == 1;
                }
                else
                {
                    /*
                    var rowsAffected = connection.Execute("Update Agent SET field1 = @param1 WHERE Agent.PersonID = @personID",
                        new { personID = agent.PersonID });
                    result = rowsAffected == 1;*/
                    result = true;
                }
                return result;
            }

        }

        public bool Delete(string personId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute("DELETE FROM Scout WHERE PersonID = @personID ",
                    new { personId });
                return rowsAffected == 1;
            }
        }
    }
}

