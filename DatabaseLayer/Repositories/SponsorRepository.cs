using Dapper;
using DatabaseLayer.DBSettings;
using System.Collections.Generic;
using System.Data.SQLite;

namespace DatabaseLayer.Repositories
{
    public class SponsorRepository
    {
        public List<Sponsor> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                return connection.Query<Sponsor>("SELECT * FROM Sponsor").AsList();
            }
        }
        public Sponsor Retrieve(int ID)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                return connection.QueryFirstOrDefault<Sponsor>(
                    @"SELECT * FROM Sponsor
                      WHERE ID = @ID",new { ID });
            }
        }
        public bool Insert(string Name)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Team>("SELECT * FROM Sponsor WHERE Name = @Name", new { Name });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute(
                        @"INSERT INTO Sponsor (ID, Name) 
                          VALUES (@Name)", new { Name});
                    result = rowsAffected == 1;
                }
                return result;
            }
        }
        public bool Delete(int ID)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute("DELETE FROM Sponsor WHERE ID = @ID",
                    new { ID });
                return rowsAffected == 1;
            }
        }
    }
}
