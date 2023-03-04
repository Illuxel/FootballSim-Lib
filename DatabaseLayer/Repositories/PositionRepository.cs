using DatabaseLayer.DBSettings;
using System.Collections.Generic;
using Dapper;
using System.Data.SQLite;


namespace FootBalLife.Database.Repositories
{
    public class PositionRepository
    {
        public List<Position> Retrive()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                return connection.Query<Position>("SELECT * FROM Position").AsList();
            }
        }
        public Position Retrive(string positionCode)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Position>("SELECT * FROM Position Where ID = @positionCode", new { positionCode });
            }
        }

        internal bool Insert(Position position)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Position>("SELECT * FROM Position WHERE ID = @positionCode", new { positionCode = position.Id });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute("INSERT INTO Position (ID, Location, Name) VALUES (@Id, @Location, @Name)",
                        position);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        internal bool Update(Position position)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Position>("SELECT * FROM Position WHERE ID = @positionCode", new { positionCode = position.Id });
                bool result = false;
                if (record != null)
                {
                    var rowsAffected = connection.Execute("UPDATE Position SET Location = @Location, Name =  @Name WHERE ID = @Id",
                        position);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }
        public bool Delete(string positionCode)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute("DELETE FROM Position WHERE ID = @positionCode ",
                    new { positionCode });
                return rowsAffected == 1;
            }
        }
    }
}


