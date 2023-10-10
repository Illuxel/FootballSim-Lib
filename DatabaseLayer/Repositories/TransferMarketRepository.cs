using System.Data.SQLite;
using System.Collections.Generic;

using Dapper;

using DatabaseLayer.Settings;
using DatabaseLayer.Model;

namespace DatabaseLayer.Repositories
{
    public class TransferMarketRepository
    {
        public List<TransferMarket> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.Query<TransferMarket>("SELECT * FROM TransferMarket").AsList();
            }
        }

        public List<TransferMarket> RetrieveByPlayer(string playerId)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.Query<TransferMarket>("SELECT * FROM TransferMarket Where IDPlayer = @playerId", new { playerId }).AsList();
            }
        }

        public List<TransferMarket> RetrieveByParameters(TransferMarketSearchParams transfer)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();



                var query = @"SELECT TM.* FROM TransferMarket TM 
                    LEFT JOIN Player P ON P.PersonId = TM.IDPlayer 
                    LEFT JOIN Person Pers ON Pers.ID = TM.IDPlayer 
                    Where P.Rating >= @Ratting  AND (((julianday('now') - julianday(Pers.Birthday))/365.25) BETWEEN @AgeLowerBound  AND @AgeUpperBound ) 
                    AND (TM.DesireAmount BETWEEN @SumLowerBound  AND @SumUpperBound ) AND P.PositionCode in @PositionParams ";

                return connection.Query<TransferMarket>(query, transfer).AsList();
            }
        }
        public TransferMarket Retrieve(string id)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<TransferMarket>("SELECT * FROM TransferMarket Where ID = @id", new { id });
            }
        }

        public bool Insert(TransferMarket transfer)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<TransferMarket>("SELECT * FROM TransferMarket WHERE ID = @id", new { id = transfer.Id });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute("INSERT INTO TransferMarket (ID, IDPlayer, IDTeam, AGREEMENT, DESIREAMOUNT) VALUES (@ID, @IDPlayer, @IDTeam, @AGREEMENT, @DESIREAMOUNT)",
                        transfer);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        public bool Update(TransferMarket transfer)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<TransferMarket>("SELECT * FROM TransferMarket WHERE ID = @transfer", new { transfer = transfer.Id });
                bool result = false;
                if (record != null)
                {
                    var rowsAffected = connection.Execute("UPDATE TransferMarket SET IDPlayer = @IDPlayer, IDTeam = @IDTeam, AGREEMENT = @AGREEMENT, DESIREAMOUNT = @DESIREAMOUNT WHERE ID = @Id",
                        transfer);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }
        public bool Delete(string transfer)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute("DELETE FROM TransferMarket WHERE ID = @transfer ",
                    new { transfer });
                return rowsAffected == 1;
            }
        }
    }
}
