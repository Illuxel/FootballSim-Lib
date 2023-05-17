using DatabaseLayer.Model;
using System.Collections.Generic;
using System.Data.SQLite;
using DatabaseLayer.DBSettings;
using Dapper;
using System.Linq;
using System.Data;
using System;

namespace DatabaseLayer.Repositories
{
    public class ActiveSponsorContractRepository
    {
        List<ActiveSponsorContract> _expired;
        ActiveSponsorContract _contract;

        public ActiveSponsorContractRepository()
        {
            _expired = new List<ActiveSponsorContract>();
            _contract = new ActiveSponsorContract(); 
        }


        public List<ActiveSponsorContract> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                return connection.Query<ActiveSponsorContract>("SELECT * FROM ActiveSponsorContract").AsList();
            }
        }
        public List<ActiveSponsorContract> Retrieve(string TeamID) 
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                return connection.Query<ActiveSponsorContract>(
                    @"SELECT * FROM ActiveSponsorContract
                      WHERE TeamID = @TeamID", new { TeamID }).AsList();
            }
        }
        public bool Insert(ActiveSponsorContract contract)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var response = connection.QueryFirstOrDefault<ActiveSponsorContract>(
                    @"SELECT * FROM ActiveSponsorContract
                    WHERE TeamID = @TeamID 
                    AND SeasonFrom = @SeasonFrom 
                    AND SeasonTo = @SeasonTo 
                    AND Value = @Value 
                    AND SponsorID = @SponsorID",
                    contract);

                bool result = false;

                if (response == null)
                {
                    var rowsAffected = connection.Execute(
                        @"INSERT INTO ActiveSponsorContract 
                        (ID,TeamID,SeasonFrom,SeasonTo,Value,SponsorID) 
                        VALUES(@ID,@TeamID,@SeasonFrom,@SeasonTo,@Value,@SponsorID)", 
                        contract);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }


        private string ConvertToSeason(int year)
        {
            return string.Format("{0}/{1}", year, year + 1);
        }



        public bool DeleteExpired(int gameYear)
        {
            var season = ConvertToSeason(gameYear);
            var contracts = Retrieve().OrderBy(x=>x.SeasonTo).ToList();

            if(contracts == null)
            {
                return false;
            }

            _contract = contracts.Where(x => x.SeasonTo == season).FirstOrDefault();
            if(_contract == null)
            {
                return false;
            }

            int index = contracts.IndexOf(_contract);


            for (int i = 0; i < index; i++)
            {
                _expired.Add(contracts[i]);
            }

            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var rowsAffected = connection.Execute(
                            "DELETE FROM Sponsor WHERE ID = @ID",
                        new { ID = _expired.Select(x=>x.ID)},transaction);
                        transaction.Commit();

                        return rowsAffected == 1;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

    }
}
