using Dapper;
using DatabaseLayer.DBSettings;
using DatabaseLayer.Model;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace DatabaseLayer.Repositories
{
    public class ManagerRequestOfPlayersRepository
    {
        public List<ManagerRequestOfPlayers> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var response = connection.Query<ManagerRequestOfPlayers>("SELECT * FROM ManagerRequestOfPlayers").ToList();
                return response;
            }
        }

        public ManagerRequestOfPlayers Retrieve(string Id)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var response = connection.QueryFirstOrDefault<ManagerRequestOfPlayers>(@"
                SELECT * FROM ManagerRequestOfPlayers 
                WHERE Id = @Id", Id);
                return response;
            }
        }

        public bool Insert(ManagerRequestOfPlayers request)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var createdDate = request.CreatedDate.ToString("dd-MM-yyyy");
                var criteria = request.Criteria.ToString(); 
                connection.Open();
                var rowsAffected = connection.Execute(
                    @"INSERT INTO ManagerRequestOfPlayers (Id, ManagerId, TeamId, Status, CreatedDate, Criteria)
                    VALUES (@Id, @ManagerId, @TeamId, @Status, @CreatedDate, @Criteria)",
                    new
                    {
                        request.ManagerId,
                        request.TeamId,
                        request.Status,
                        createdDate,
                        criteria
                    });
                return rowsAffected == 1;
            }
        }

        public bool Update(ManagerRequestOfPlayers request)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var createdDate = request.CreatedDate.ToString("dd-MM-yyyy");
                var criteria = request.Criteria.ToString();
                connection.Open();
                var rowsAffected = connection.Execute(
                    @"UPDATE ManagerRequestOfPlayers 
                    SET ManagerId = @ManagerId, 
                    TeamId = @TeamId, 
                    Status = @Status, 
                    CreatedDate = @CreatedDate, 
                    Criteria = @Criteria
                    WHERE Id = @Id",
                    new
                    {
                        request.ManagerId,
                        request.TeamId,
                        request.Status,
                        createdDate,
                        criteria
                    });

                return rowsAffected == 1;
            }
        }
    }
}
