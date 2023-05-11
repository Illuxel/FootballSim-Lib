using System.Collections.Generic;
using System;
using DatabaseLayer.DBSettings;
using System.Data.SQLite;
using Dapper;
using System.Linq;
using System.Data;

namespace DatabaseLayer.Repositories
{
    public class MatchRepository
    {
        public List<Match> Retrieve(string teamId)
        {
            List<Match> result = new List<Match>();
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                result = connection.Query<Match>(
                    "SELECT * FROM Match WHERE HomeTeamId = @teamId or GuestTeamId = @teamId",
                    new { teamId }).AsList();
            }

            return result;
        }

        public List<Match> Retrieve(int leagueId, int tourNumber = 0)
        {
            List<Match> result = new List<Match>();
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM Match WHERE LeagueId = @leagueId";
                var sqlParams = new { leagueId };
                
                result = connection.Query<Match>(sql, sqlParams).AsList();
                if (tourNumber != 0)
                {
                    return result.Where(item => item.TourNumber == tourNumber).ToList();
                }
            }

            return result;
        }

        public void Insert(List<Match> matches)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var rowsAffected = connection.Execute(
                        @"INSERT INTO Match (Id, HomeTeamId, GuestTeamId, MatchDate, HomeTeamGoals, 
                            GuestTeamGoals, TourNumber, LeagueId)
                            VALUES (@Id, @HomeTeamId, @GuestTeamId, @MatchDate, @HomeTeamGoals, 
                            @GuestTeamGoals, @TourNumber, @LeagueId)",
                        matches, transaction);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public bool Insert(Match match)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                match.Id = Guid.NewGuid().ToString();
                var record = connection.QuerySingleOrDefault<Match>("SELECT * FROM Match WHERE Id = @id",
                    new { id = match.Id });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute(
                        @"INSERT INTO Match (Id, HomeTeamId, GuestTeamId, MatchDate, HomeTeamGoals, 
                           GuestTeamGoals, TourNumber, LeagueId)
                         VALUES (@Id, @HomeTeamId, @GuestTeamId, @MatchDate, @HomeTeamGoals, 
                           @GuestTeamGoals, @TourNumber, @LeagueId)",
                        match);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        /*public void Insert(List<Match> matches)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                foreach(var match in matches)
                {
                    match.Id = Guid.NewGuid().ToString();
                }
                connection.Insert(matches);

                //return rowsAffected == 1;
            }
        }*/

        public bool Update(Match match)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Match>("SELECT * FROM Match WHERE Id = @id",
                    new { id = match.Id });
                bool result = false;
                if (record != null)
                {
                    var rowsAffected = connection.Execute(
                        @"UPDATE Match
                            SET HomeTeamId = @HomeTeamId,
                                GuestTeamId = @GuestTeamId,
                                MatchDate = @MatchDate,
                                HomeTeamGoals = @HomeTeamGoals,
                                GuestTeamGoals = @GuestTeamGoals,
                                TourNumber = @TourNumber,
                                LeagueId = @LeagueId
                            WHERE d = @Id;",
                        match);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }
    }
}

