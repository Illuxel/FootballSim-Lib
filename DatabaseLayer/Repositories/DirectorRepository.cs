using DatabaseLayer.DBSettings;
using System.Collections.Generic;
using Dapper;
using System.Data.SQLite;
using System.Linq;

namespace DatabaseLayer.Repositories
{
    public class DirectorRepository
    {

        //TODO: Чи потрібні всі директори?
        public List<Director> Retrieve()
        {

            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();

                var directors = connection.Query<Director>("SELECT * FROM Director").AsList();

                var persons = connection.Query<Person>("SELECT * FROM Person WHERE ID IN @ids",
                    new { ids = directors.Select(item => item.PersonID) }).ToDictionary(pers => pers.Id, pers => pers);
                foreach (var director in directors)
                {
                    if (persons.TryGetValue(director.PersonID, out Person person))
                    {
                        director.Person = person;
                    }
                }
                return directors;
            }
        }
        public Director Retrieve(string personId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var director = connection.QueryFirstOrDefault<Director>("SELECT * FROM Director WHERE PersonID = @personId", new { personId });
                if (director != null && !string.IsNullOrEmpty(director.PersonID))
                {
                    var person = connection.QueryFirstOrDefault<Person>("SELECT * FROM Person WHERE ID = @personId", new { personId });
                    if (person != null)
                    {
                        director.Person = person;
                    }
                }
                return director;
            }
        }


        //TODO: Дописати Modify, продумати які параметри будуть у агента
        public bool Modify(Director director)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Director>("SELECT * FROM Director WHERE PersonID = @personID", new { personID = director.PersonID });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute("INSERT INTO Director (PersonID) VALUES (@personID)",
                        new { personID = director.PersonID });
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
                var rowsAffected = connection.Execute("DELETE FROM Director WHERE PersonID = @personID ",
                    new { personId });
                return rowsAffected == 1;
            }
        }
    }
}

