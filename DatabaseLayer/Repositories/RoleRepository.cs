using Dapper;
using DatabaseLayer.DBSettings;
using System.Data.SQLite;
using System.Collections.Generic;

namespace FootBalLife.Database.Repositories
{
    public class RoleRepository
    {
        public List<Role> Retrive()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                return connection.Query<Role>("SELECT * FROM Role").AsList();
            }
        }
        public Role Retrive(int roleId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Role>("SELECT * FROM Role Where ID = @roleId", new { roleId });
            }
        }

        internal bool Insert(Role role)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Role>("SELECT * FROM Role WHERE ID = @roleId", new { roleId = role.Id });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute(
                        "INSERT INTO Role (ID, isNpc, Icon, Name) VALUES (@Id, @IsNpc, @Icon, @Name)", role);
                    result = rowsAffected == 1;
                }
                return result;
            }

        }

        public bool Update(Role role)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Role>("SELECT * FROM Role WHERE ID = @roleId", new { roleId = role.Id });
                bool result = false;
                if (record != null)
                {
                    var rowsAffected = connection.Execute(
                        "UPDATE Role SET isNpc = @IsNpc, Icon = @Icon, Name = @Name WHERE ID = @Id", role);
                    result = rowsAffected == 1;
                }
                return result;
            }

        }
        public bool Delete(int roleId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute("DELETE FROM Role WHERE PersonID = @roleId ",
                    new { roleId });
                return rowsAffected == 1;
            }
        }
    }
}

