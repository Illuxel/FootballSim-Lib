using System;
using System.Data.SQLite;
using System.Collections.Generic;

using Dapper;

using DatabaseLayer.Settings;
using DatabaseLayer.Enums;
using DatabaseLayer.Model;

namespace DatabaseLayer.Repositories
{
    public class SponsorCreateRequestRepository
    {
        public List<SponsorCreateRequest> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.Query<SponsorCreateRequest>("SELECT * FROM ActiveSponsorContract").AsList();
            }
        }

        public List<SponsorCreateRequest> Retrieve(string TeamID) 
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.Query<SponsorCreateRequest>(
                    @"SELECT * FROM ActiveSponsorContract
                      WHERE TeamID = @TeamID", new { TeamID }).AsList();
            }
        }

        public List<Sponsor> RetrieveFreeSponsor(string teamId)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                return connection.Query<Sponsor>(@"SELECT sponsor.* 
                FROM Sponsor sponsor
                WHERE sponsor.ID 
                NOT IN 
                (SELECT SponsorID 
                FROM ActiveSponsorContract 
                WHERE TeamID = @TeamId)", new { TeamId = teamId }).AsList();
            }
        }
            
        public bool Insert(SponsorCreateRequest contract)
        {
            var result = false;
            if (string.IsNullOrEmpty(contract.ID))
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                contract.ID = Guid.NewGuid().ToString();
                connection.Open();

                var rowsAffected = connection.Execute(
                    @"INSERT INTO ActiveSponsorContract 
                    (ID,TeamID,SeasonFrom,SeasonTo,Value,SponsorID,State) 
                    VALUES(@ID,@TeamID,@SeasonFrom,@SeasonTo,@Value,@SponsorID,@State)",
                    contract);
                result = rowsAffected == 1;
            }

            return result;
        }

        public bool UpdateState(SponsorCreateRequest contract)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<SponsorCreateRequest>(
                    @"SELECT * FROM SponsorCreateRequest
                    WHERE ID = @id", new { id = contract.ID });
                bool result = false;
                if (record != null)
                {
                    var rowsAffected = connection.Execute(
                        @"UPDATE SponsorCreateRequest SET
                        State = @State
                        WHERE ID = @ID",contract);
                    return rowsAffected == 1;
                }
                return result;
            }
        }

        public bool Delete(string ID)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute(
                            @"DELETE FROM ActiveSponsorContract 
                            WHERE ID = @id",
                            new { id = ID });

                return rowsAffected == 1;
            }
        }

        public bool DeleteExpired(string expiredSeason)
        {
            
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute(
                            @"DELETE FROM ActiveSponsorContract 
                            WHERE SeasonTo = @expiredSeason",
                            new {expiredSeason});

                return rowsAffected == 1;
            }
        }

        public bool DeleteCanceled(string teamId)
        {
            using (var connection = new SQLiteConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute(
                            @"DELETE FROM ActiveSponsorContract 
                            WHERE TeamID = @teamId 
                            AND State = @status",
                            new {TeamID = @teamId,status = SponsorRequestStatus.Waiting});

                return rowsAffected == 1;
            }
        }
    }
}
