using System.Linq;
using System.Data.SQLite;
using System.Collections.Generic;

using Dapper;

using DatabaseLayer.Settings;

namespace DatabaseLayer.Repositories
{
    public class CoachRepository
    {
        public List<Coach> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();

                var coaches = connection.Query<Coach>("SELECT * FROM Coach").AsList();

                var persons = connection.Query<Person>("SELECT * FROM Person WHERE ID IN @ids",
                    new { ids = coaches.Select(item => item.PersonID) }).ToDictionary(pers => pers.Id, pers => pers);
                foreach (var coach in coaches)
                {
                    if (persons.TryGetValue(coach.PersonID, out Person person))
                    {
                        coach.Person = person;
                    }
                }
                return coaches;
            }
        }
        public Coach Retrieve(string personId)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var agent = connection.QueryFirstOrDefault<Coach>("SELECT * FROM Coach WHERE PersonID = @personId", new { personId });
                if (agent != null && !string.IsNullOrEmpty(agent.PersonID))
                {
                    var person = connection.QueryFirstOrDefault<Person>("SELECT * FROM Person WHERE ID = @personId", new { personId });
                    if (person != null)
                    {
                        agent.Person = person;
                    }
                }
                return agent;
            }
        }
        //TODO: Дописати Modify, продумати які параметри будуть у агента
        public bool Modify(Coach coach)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Agent>(
                    "SELECT * FROM Agent WHERE PersonID = @personID", new { personID = coach.PersonID });
                bool result;
                if (record == null)
                {
                    var rowsAffected = connection.Execute("INSERT INTO Agent (PersonID) VALUES (@personID)",
                        new { personID = coach.PersonID });
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
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute("DELETE FROM Coach WHERE PersonID = @personID ",
                    new { personId });
                return rowsAffected == 1;
            }
        }
    }
}
