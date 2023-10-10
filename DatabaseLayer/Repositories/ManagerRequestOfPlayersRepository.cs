using System.Linq;
using System.Data.SQLite;
using System.Collections.Generic;

using Dapper;

using DatabaseLayer.Model;
using DatabaseLayer.Settings;

namespace DatabaseLayer.Repositories
{
    public class ManagerRequestOfPlayersRepository
    {
        public List<ManagerRequestOfPlayers> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var response = connection.Query<ManagerRequestOfPlayers>("SELECT * FROM ManagerRequestOfPlayers").ToList();
                return response;
            }
        }

        public List<ManagerRequestOfPlayers> Retrieve(string TeamId)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var response = connection.Query<ManagerRequestOfPlayers>(
                    @"SELECT * FROM ManagerRequestOfPlayers WHERE TeamId = @TeamId",
                    new { TeamId }).ToList();
                return response;
            }
        }
        public ManagerRequestOfPlayers RetrieveById(string Id)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var response = connection.Query<ManagerRequestOfPlayers>(
                    @"SELECT * FROM ManagerRequestOfPlayers WHERE Id = @Id",
                    new { Id }).First();
                return response;
            }
        }

        public bool Insert(ManagerRequestOfPlayers request)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                var createdDate = request.CreatedDate.ToString("yyyy-MM-dd");
                var finishDate = request.FinishDate.ToString("yyyy-MM-dd");
                connection.Open();
                var rowsAffected = connection.Execute(
                    @"INSERT INTO ManagerRequestOfPlayers (Id, ManagerId, TeamId, Status, BudgetLimit, CreatedDate, FinishDate, PlayerId, CriteriaJSON)
            VALUES (@Id, @ManagerId, @TeamId, @Status, @BudgetLimit, @CreatedDate, @FinishDate, @PlayerId, @CriteriaJSON)",
                    new
                    {
                        request.Id,
                        request.ManagerId,
                        request.TeamId,
                        request.Status,
                        request.BudgetLimit,
                        CreatedDate = createdDate,
                        FinishDate = finishDate,
                        request.PlayerId,
                        request.CriteriaJSON
                    });
                return rowsAffected == 1;
            }
        }

        public bool Update(ManagerRequestOfPlayers request)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                var createdDate = request.CreatedDate.ToString("yyyy-MM-dd");
                var finishDate = request.FinishDate.ToString("yyyy-MM-dd");
                connection.Open();
                var rowsAffected = connection.Execute(
                    @"UPDATE ManagerRequestOfPlayers 
                    SET ManagerId = @ManagerId, 
                    TeamId = @TeamId, 
                    Status = @Status, 
                    BudgetLimit = @BudgetLimit,
                    CreatedDate = @CreatedDate,
                    FinishDate = @FinishDate,
                    PlayerId = @PlayerId,
                    CriteriaJSON = @CriteriaJSON
                    WHERE Id = @Id",
                    new
                    {
                        request.Id,
                        request.ManagerId,
                        request.TeamId,
                        request.Status,
                        request.BudgetLimit,
                        CreatedDate = createdDate,
                        FinishDate = finishDate,
                        request.PlayerId,
                        request.CriteriaJSON
                    });

                return rowsAffected == 1;
            }
        }
    }
}
