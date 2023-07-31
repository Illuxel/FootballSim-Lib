using Dapper;
using DatabaseLayer.DBSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Net;

namespace DatabaseLayer.Repositories
{
    public class NationalResTabRepository
    {
        public List<NationalResultTable> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                return connection.Query<NationalResultTable>("SELECT * FROM NationalResultTable").AsList();
            }
        }
        public List<NationalResultTable> Retrieve(long leagueId, string season)
        {
            var result = new List<NationalResultTable>();
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                result = connection.Query<NationalResultTable, Team, NationalResultTable>(
                    "SELECT e.*, t.* " +
                    "from NationalResultTable e " +
                    "inner join Team t on e.TeamId = t.ID " +
                    "WHERE t.LeagueId = @leagueId AND e.Season = @season;",
                    (nationalTable, team) =>
                    {
                        nationalTable.Team = team;
                        return nationalTable;
                    },
                    param: new { leagueId, season },
                    splitOn: "ID").AsList();
            }

            return result;
        }

        public bool Update(NationalResultTable model)
        {
            bool result = false;
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();

                var rowAffected = connection.Execute(
                     "UPDATE NationalResultTable SET "+
                     "Wins = @wins, Draws = @draws,"+
                     "Loses = @loses, ScoredGoals = @scoredGoals, MissedGoals = @missedGoals,"+
                     "TotalPosition = @totalPosition, TotalPoints = @totalPoints WHERE TeamId = @teamId AND Season = @season", 
                     param: new { model });
                result = rowAffected == 1;
            }

            return result;
        }

        public List<NationalResultTable> Retrieve(string teamId, string season)
        {
            var result = new List<NationalResultTable>();
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                result = connection.Query<NationalResultTable, Team, NationalResultTable>(
                    "SELECT e.*, t.* " +
                    "from NationalResultTable e " +
                    "inner join Team t on e.TeamId = t.ID " +
                    "WHERE t.ID = @teamId AND e.Season = @season;",
                    (nationalTable, team) =>
                    {
                        nationalTable.Team = team;
                        return nationalTable;
                    },
                    param: new { teamId, season },
                    splitOn: "ID").AsList();
            }

            return result;
        }

        public Dictionary<string, NationalResultTable> Retrieve(string season)
        {
            var results = new Dictionary<string, NationalResultTable>();

            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();

                string query = @"SELECT * FROM NationalResultTable
                         WHERE Season = @season";

                var queryResult = connection.Query<NationalResultTable>(
                    query, new { season }).AsList();

                foreach (var result in queryResult)
                {
                    if (!results.ContainsKey(result.TeamID))
                    {
                        results[result.TeamID] = result;
                    }
                }
            }

            return results;
        }

        public bool Insert(NationalResultTable model)
        {
            bool result = false;
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();

                var rowAffected = connection.Execute(
                    @"INSERT INTO NationalResultTable (Season, TeamId, Wins, Draws, Loses, ScoredGoals, MissedGoals, TotalPosition, TotalPoints)
                    VALUES (@Season, @TeamId, @Wins, @Draws, @Loses, @ScoredGoals, @MissedGoals, @TotalPosition, @TotalPoints)", model
                    );

                result = rowAffected == 0;
            }

            return result;
        }

        public bool Delete(string teamId, string season)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var rowAffected = connection.Execute("DELETE FROM NationalResultTable WHERE TeamID = @TeamId AND Season = @Season",
                    param: new { TeamId = teamId, Season = season });

                return rowAffected == 1;
            }
        }
        public bool Update(NationalResultTable homeTeam, NationalResultTable guestTeam, string season)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();

                var rowAffected = connection.Execute(
                    "UPDATE NationalResultTable SET " +
                    "Wins = @Wins, Draws = @Draws, Loses = @Loses, " +
                    "ScoredGoals = @ScoredGoals, MissedGoals = @MissedGoals, " +
                    "TotalPosition = @TotalPosition " +
                    "TotalPoints = @TotalPoints"+
                    "WHERE TeamId = @TeamId AND Season = @Season",
                    new
                    {
                        homeTeam.Wins,
                        homeTeam.Draws,
                        homeTeam.Loses,
                        homeTeam.ScoredGoals,
                        homeTeam.MissedGoals,
                        homeTeam.TotalPosition,
                        homeTeam.TeamID,
                        homeTeam.TotalPoints,
                        Season = season
                    });

                if (rowAffected == 1)
                {
                    rowAffected = connection.Execute(
                        "UPDATE NationalResultTable SET " +
                        "Wins = @Wins, Draws = @Draws, Loses = @Loses, " +
                        "ScoredGoals = @ScoredGoals, MissedGoals = @MissedGoals, " +
                        "TotalPosition = @TotalPosition, TotalPoints = @TotalPoints " +
                        "WHERE TeamId = @TeamId AND Season = @Season",
                        new
                        {
                            guestTeam.Wins,
                            guestTeam.Draws,
                            guestTeam.Loses,
                            guestTeam.ScoredGoals,
                            guestTeam.MissedGoals,
                            guestTeam.TotalPosition,
                            guestTeam.TeamID,
                            guestTeam.TotalPoints,
                            Season = season
                        });

                    return rowAffected == 1;
                }

                return false;
            }
        }

        public bool Update(List<NationalResultTable> teamResultTab, string season)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var rowAffected = connection.Execute(
                            "UPDATE NationalResultTable " +
                            "SET Wins = @Wins, " +
                            "Draws = @Draws, " +
                            "Loses = @Loses, " +
                            "ScoredGoals = @ScoredGoals, " +
                            "MissedGoals = @MissedGoals, " +
                            "TotalPosition = @TotalPosition, " +
                            "TotalPoints = @TotalPoints " +
                            "WHERE TeamId = @TeamId AND Season = @Season",
                            teamResultTab.Select(res => new
                            {
                                res.Wins,
                                res.Draws,
                                res.Loses,
                                res.ScoredGoals,
                                res.MissedGoals,
                                res.TotalPosition,
                                res.TotalPoints,
                                res.TeamID,
                                Season = season
                            }),
                            transaction);
                        transaction.Commit();
                        return rowAffected != 0;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }
    }
}
