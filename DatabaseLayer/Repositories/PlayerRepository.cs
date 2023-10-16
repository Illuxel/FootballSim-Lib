using Dapper;
using DatabaseLayer.DBSettings;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System;

namespace DatabaseLayer.Repositories
{

    public class PlayerRepository
    {
        public List<Player> Retrieve()
        {
            return retrieve("1=1");
        }

        public List<Player> Retrieve(string teamId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                return retrieve("Contract.TeamID = @teamId", new { teamId });
            }
        }

        public Dictionary<string, Player> Retrieve(List<string> playerIds)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var players = retrieve("Contract.PersonID in @playerIds", new { playerIds });
                return players.ToDictionary(p => p.PersonID, p => p);
            }
        }

        public List<Player> RetrieveJuniorsByTeam(string teamId)
        {
            return retrieve("Contract.TeamID = @teamId AND IsJunior = 1", new { teamId });
        }

        public List<Player> RetrieveAllJuniors()
        {
            return retrieve("IsJunior = 1");
        }

        private List<Player> retrieve(string condition, object queryParams = null)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var query = string.Format(@"SELECT Player.*, Person.*, Position.*
                    FROM Player
                    INNER JOIN Person ON Player.PersonID = Person.ID
                    LEFT JOIN Position ON Player.PositionCode = Position.Code
                    INNER JOIN Contract ON Contract.PersonID = Person.ID
                    WHERE {0}", condition);
                var players = connection.Query<Player>(query, param: queryParams);
                var positionQuery = @"SELECT * FROM Position";
                var potitions = connection.Query<Position>(positionQuery);
                var personQuery = @"SELECT * FROM Person WHERE ID IN @ids";
                var persons = connection.Query<Person>(
                    personQuery,
                    param: new { ids = players.Select(item => item.PersonID) }
                );

                foreach (var player in players)
                {
                    player.Position = potitions.Where(item => item.Code == player.PositionCode).FirstOrDefault();
                    player.Person = persons.Where(item => item.Id == player.PersonID).FirstOrDefault();

                    player.CurrentPlayerRating = player.Rating;
                }
                return players.AsList();
            }
        }


        public List<Player> RetrieveByScout(string personId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                return retrieve("PLAYER.FINDBYSCOUT = @personId", new { personId });
            }
        }
        public Player RetrieveOne(string personId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var query = @"SELECT Player.*, Person.*, Position.*
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
                    splitOn: "ID, CODE"
                    );

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
                        @"INSERT INTO Player (PersonID, PositionCode, ContractId, Speed, 
                            Endurance, Strike, Physics, Defending, Passing, Dribbling, Rating, IndexPosition, 
                            CurrentRating, PlayerPositionGroup, IsJunior, InjuredTo) 
                        VALUES (@PersonID, @PositionCode, @ContractId, @Speed,
                            @Endurance, @Strike, @Physics, @Defending, @Passing, @Dribbling, @Rating, @IndexPosition, 
                            @Rating, @PlayerPositionGroup, @IsJunior, @InjuredTo)",
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
                        @"INSERT INTO Player (PersonID, PositionCode, ContractId, Speed, 
                            Endurance, Strike, Physics, Defending, Passing, Dribbling, Rating, IndexPosition, 
                            CurrentRating, PlayerPositionGroup, IsJunior, InjuredTo) 
                        VALUES (@PersonID, @PositionCode, @ContractId, @Speed,
                            @Endurance, @Strike, @Physics, @Defending, @Passing, @Dribbling, @Rating, @IndexPosition, 
                            @Rating, @PlayerPositionGroup, @IsJunior, @InjuredTo)",
                        players, transaction);
                        transaction.Commit();
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
                            Endurance = @Endurance, 
                            Strike = @Strike, 
                            Physics = @Physics, 
                            Defending = @Defending, 
                            Passing = @Passing,
                            Dribbling = @Dribbling,
                            Rating = @Rating,
                            IndexPosition = @IndexPosition, 
                            CurrentRating = @CurrentPlayerRating, 
                            PlayerPositionGroup = @PlayerPositionGroup,
                            InjuredTo = @InjuredTo,
                            IsJunior = @IsJunior    
                        WHERE PersonID = @PersonID",
                        player);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        public bool Update(List<Player> players)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute(@"UPDATE Player 
                        SET PositionCode = @PositionCode, 
                            ContractId = @ContractId, 
                            Speed = @Speed, 
                            Endurance = @Endurance, 
                            Strike = @Strike, 
                            Physics = @Physics, 
                            Defending = @Defending, 
                            Passing = @Passing,
                            Dribbling = @Dribbling,
                            Rating = @Rating,
                            IndexPosition = @IndexPosition, 
                            CurrentRating = @CurrentPlayerRating, 
                            PlayerPositionGroup = @PlayerPositionGroup,
                            InjuredTo = @InjuredTo,
                            IsJunior = @IsJunior
                        WHERE PersonID = @PersonID",
                        players, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
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
