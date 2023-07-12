﻿using Dapper;
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

        public List<ManagerRequestOfPlayers> Retrieve(string TeamId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var response = connection.Query<ManagerRequestOfPlayers>(
                    @"SELECT * FROM ManagerRequestOfPlayers WHERE TeamId = @TeamId",
                    new { TeamId }).ToList();
                return response;
            }
        }

        public bool Insert(ManagerRequestOfPlayers request)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var createdDate = request.CreatedDate.ToString("yyyy-MM-dd");
                var criteria = request.Criteria.ToString();
                connection.Open();
                var rowsAffected = connection.Execute(
                    @"INSERT INTO ManagerRequestOfPlayers (Id, ManagerId, TeamId, Status, CreatedDate, CriteriaJSON)
            VALUES (@Id, @ManagerId, @TeamId, @Status, @CreatedDate, @CriteriaJSON)",
                    new
                    {
                        request.Id,
                        request.ManagerId,
                        request.TeamId,
                        request.Status,
                        CreatedDate = createdDate,
                        request.CriteriaJSON
                    });
                return rowsAffected == 1;
            }
        }

        public bool Update(ManagerRequestOfPlayers request)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var createdDate = request.CreatedDate.ToString("yyyy-MM-dd");
                connection.Open();
                var rowsAffected = connection.Execute(
                    @"UPDATE ManagerRequestOfPlayers 
                    SET ManagerId = @ManagerId, 
                    TeamId = @TeamId, 
                    Status = @Status, 
                    CreatedDate = @CreatedDate, 
                    CriteriaJSON = @CriteriaJSON
                    WHERE Id = @Id",
                    new
                    {
                        request.ManagerId,
                        request.TeamId,
                        request.Status,
                        createdDate,
                        request.CriteriaJSON,
                        request.Id
                    });

                return rowsAffected == 1;
            }
        }
    }
}
