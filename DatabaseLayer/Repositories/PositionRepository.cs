using System.Data.SQLite;
using System.Collections.Generic;

using Dapper;
using DatabaseLayer.Settings;

namespace DatabaseLayer.Repositories
{
    public class PositionRepository
    {
        public List<Position> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.Query<Position>("SELECT * FROM Position").AsList();
            }
        }
        public Position Retrieve(string positionCode)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Position>("SELECT * FROM Position Where Code = @positionCode", new { positionCode });
            }
        }

        internal bool Insert(Position position)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Position>("SELECT * FROM Position WHERE Code = @positionCode", new { positionCode = position.Code });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute("INSERT INTO Position (Code, Location, Name) VALUES (@Code, @Location, @Name)",
                        position);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        internal bool Update(Position position)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Position>("SELECT * FROM Position WHERE Code = @positionCode", new { positionCode = position.Code });
                bool result = false;
                if (record != null)
                {
                    var rowsAffected = connection.Execute("UPDATE Position SET Location = @Location, Name =  @Name WHERE Code = @Code",
                        position);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }
        public bool Delete(string positionCode)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute("DELETE FROM Position WHERE Code = @positionCode ",
                    new { positionCode });
                return rowsAffected == 1;
            }
        }
    }
}


