using DatabaseLayer.DBSettings;
using System.Collections.Generic;
using Dapper;
using System.Data.SQLite;
using System.Linq;

namespace DatabaseLayer.Repositories
{
    public class AgentRepository
    {
        //TODO: Чи потрібні всі агенти?
        public List<Agent> Retrive()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var agents = connection.Query<Agent>("SELECT * FROM Agent").AsList();

                var persons = connection.Query<Person>("SELECT * FROM Person WHERE ID IN @ids",
                    new { ids = agents.Select(item => item.PersonID) }).ToDictionary(pers => pers.Id, pers => pers);
                foreach(var agent in agents)
                {
                    if(persons.TryGetValue(agent.PersonID, out Person person))
                    {
                        agent.Person = person;
                    }
                }
                return agents;
            }
        }
        public Agent Retrive(string personId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var agent = connection.QueryFirstOrDefault<Agent>("SELECT * FROM Agent WHERE PersonID = @personId", new { personId });
                if(agent != null && !string.IsNullOrEmpty(agent.PersonID))
                {
                    var person = connection.QueryFirstOrDefault<Person>("SELECT * FROM Person WHERE ID = @personId", new { personId });
                    if(person != null)
                    {
                        agent.Person = person;
                    }
                }
                return agent;
            }
        }


        public bool Insert(Agent agent)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Agent>("SELECT * FROM Agent WHERE PersonID = @personID", new { personID = agent.PersonID });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute("INSERT INTO Agent (PersonID) VALUES (@personID)", 
                        agent);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        public bool Update(Agent agent)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Agent>("SELECT * FROM Agent WHERE PersonID = @personID", new { personID = agent.PersonID });
                bool result = false;
                if (record != null)
                {
                    /*
                    var rowsAffected = connection.Execute("Update Agent SET field1 = @param1 WHERE Agent.PersonID = @personID",
                        new { personID = agent.PersonID });
                    result = rowsAffected == 1;*/
                }
                return result;
            }
        }

        public bool Delete(string personId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute("DELETE FROM Agent WHERE PersonID = @personID ",
                    new { personId });
                return rowsAffected == 1;
            }
        }
    }
}
