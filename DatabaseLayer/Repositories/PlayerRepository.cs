using Dapper;
using DatabaseLayer.DBSettings;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System;

namespace FootBalLife.Database.Repositories
{

    public class PlayerRepository
    {
        public List<Player> Retrive()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var query = @"SELECT Player.*, Person.*, Position.*
                    FROM Player
                    INNER JOIN Person ON Player.PersonID = Person.ID
                    LEFT JOIN Position ON Player.PositionCode = Position.Code";
                var results = connection.Query<Player, Person, Position, Player>(
                    query,
                    (player, person, position) =>
                    {
                        player.Person = person;
                        player.Position = position;
                        return player;
                    },
                    splitOn: "PersonID, PositionCode");

                return results.AsList();
            }
        }

        public List<Player> Retrive(string teamId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var query = @"SELECT Player.*, Person.*, c.*
                    FROM Player
                    INNER JOIN Person ON Player.PersonID = Person.ID
                    LEFT JOIN Position ON Player.PositionCode = Position.Code
                    INNER JOIN Contract ON Contract.PersonID = Person.ID
                    WHERE Contract.TeamID = @teamId";
                var results = connection.Query<Player, Person, Position, Player>(
                    query,
                    (player, person, position) =>
                    {
                        player.Person = person;
                        player.Position = position;
                        return player;
                    },
                    param: new { teamId },
                    splitOn: "PersonID, PositionCode");

                return results.AsList();
            }
        }

        public Player RetriveOne(string personId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var query = @"SELECT Player.*, Person.*, c.*
                    FROM Player
                    INNER JOIN Person ON Player.PersonID = Person.ID
                    LEFT JOIN Position ON Player.PositionCode = Position.Code
                    WHERE Person.ID = @personId";
                var results = connection.Query<Player, Person, Position, Player>(
                    query,
                    (player, person, position) =>
                    {
                        player.Person = person;
                        player.Position = position;
                        return player;
                    },
                    param: new { personId },
                    splitOn: "PersonID, PositionCode");

                return results.FirstOrDefault();
            }
        }

        public bool Insert(Player player)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Agent>("SELECT * FROM Player WHERE PersonID = @personID", new { personID = player.PersonID });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute(
                        @"INSERT INTO Player (PersonID, PositionCode, ContractId, Speed, Kick, 
                            Endurance, Strike, Physics, Technique, Passing) 
                        VALUES (@PersonID, @PositionCode, @ContractId, @Speed, @Kick, 
                            @Endurance, @Strike, @Physics, @Technique, @Passing)",
                        player);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        public void Insert(List<Player> players)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var rowsAffected = connection.Execute(
                        @"INSERT INTO Player (PersonID, PositionCode, ContractId, Speed, Kick, 
                            Endurance, Strike, Physics, Technique, Passing, Dribbling) 
                        VALUES (@PersonID, @PositionCode, @ContractId, @Speed, @Kick, 
                            @Endurance, @Strike, @Physics, @Technique, @Passing, @Dribbling)",
                        players, transaction);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public bool Update(Player player)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Agent>("SELECT * FROM Player WHERE PersonID = @personID", new { personID = player.PersonID });
                bool result = false;
                if (record != null)
                {
                    var rowsAffected = connection.Execute(@"UPDATE Player 
                        SET PositionCode = @PositionCode, 
                            ContractId = @ContractId, 
                            Speed = @Speed, 
                            Kick = @Kick, 
                            Endurance = @Endurance, 
                            Strike = @Strike, 
                            Physics = @Physics, 
                            Technique = @Technique, 
                            Passing = @Passing,
                            Dribbling = @Dribbling
                        WHERE PersonID = @PersonID",
                        player);
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
                var rowsAffected = connection.Execute("DELETE FROM Player WHERE PersonID = @personID ",
                    new { personId });
                return rowsAffected == 1;
            }
        }
    }
}
