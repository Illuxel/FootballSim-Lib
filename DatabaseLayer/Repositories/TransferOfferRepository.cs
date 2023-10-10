using System.Data.SQLite;
using System.Collections.Generic;

using Dapper;

using DatabaseLayer.Settings;
using DatabaseLayer.Model;

namespace DatabaseLayer.Repositories
{
    public class TransferOfferRepository
    {
        public List<TransferOffer> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.Query<TransferOffer>("SELECT * FROM TransferOffer").AsList();
            }
        }
        public TransferOffer Retrieve(string idTransfer)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<TransferOffer>("SELECT * FROM TransferOffer Where ID = @idTransfer", new { idTransfer });
            }
        }

        public List<TransferOffer> RetrieveByTeamSeller(string teamId)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.Query<TransferOffer>(@"SELECT TOF.* FROM TransferOffer TOF 
                    LEFT JOIN TransferMarket TM  ON TM.ID = TOF.IDMarket 
                    Where TM.IDTeam = @teamId", new { teamId }).AsList();
            }
        }

        public List<TransferOffer> RetrieveByTeamBuyer(string teamId)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.Query<TransferOffer>(@"SELECT * FROM TransferOffer 
                    Where TeamIdBuyer = @teamId", new { teamId }).AsList();
            }
        }
        public bool Insert(TransferOffer transfer)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<TransferOffer>("SELECT * FROM TransferOffer WHERE ID = @id", new { id = transfer.Id });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute("INSERT INTO TransferOffer (ID, TeamIdBuyer, IDMarket, OfferSum) VALUES (@ID, @TeamIdBuyer, @IDMarket, @OfferSum)",
                        transfer);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        public bool Update(TransferOffer transfer)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<TransferOffer>("SELECT * FROM TransferOffer WHERE ID = @transfer", new { transfer = transfer.Id });
                bool result = false;
                if (record != null)
                {
                    var rowsAffected = connection.Execute("UPDATE TransferOffer SET TeamIdBuyer = @TeamIdBuyer, OfferSum = @OfferSum, IDMarket = @IDMarket WHERE ID = @Id",
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
                var rowsAffected = connection.Execute("DELETE FROM TransferOffer WHERE ID = @transfer ",
                    new { transfer });
                return rowsAffected == 1;
            }
        }
    }
}
