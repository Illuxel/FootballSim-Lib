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
                var rowsAffected = connection.Execute(
                         @"INSERT INTO Sponsor (Name) 
                          VALUES (@Name)", new { Name });
                return rowsAffected == 1;
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
