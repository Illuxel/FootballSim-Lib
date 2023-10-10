﻿using System.Data.SQLite;
using System.Collections.Generic;

using Dapper;

using DatabaseLayer.Settings;
using DatabaseLayer.Model;

namespace DatabaseLayer.Repositories
{
    public class TransferRepository
    {
        public List<TransferJournal> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.Query<TransferJournal>("SELECT * FROM TransferJournal").AsList();
            }
        }
        public TransferJournal Retrieve(int idTransfer)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<TransferJournal>("SELECT * FROM TransferJournal Where ID = @idTransfer", new { idTransfer });
            }
        }

        public bool Insert(TransferJournal transfer)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<TransferJournal>("SELECT * FROM TransferJournal WHERE ID = @id", new { id = transfer.Id });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute("INSERT INTO TransferJournal (ID, OfferId, Status, SumFact, DateRelease) VALUES (@ID, @OfferId, @Status, @SumFact, @DateRelease)",
                        transfer);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        public bool Update(TransferJournal transfer)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<TransferJournal>("SELECT * FROM TransferJournal WHERE ID = @transfer", new { transfer = transfer.Id });
                bool result = false;
                if (record != null)
                {
                    var rowsAffected = connection.Execute("UPDATE TransferJournal SET OfferId = @OfferId, Status = @Status, SumFact = @SumFact, DateRelease = @DateRelease WHERE ID = @Id",
                        transfer);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }
        public bool Delete(int transfer)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute("DELETE FROM TransferJournal WHERE ID = @transfer ",
                    new { transfer });
                return rowsAffected == 1;
            }
        }
    }
}
