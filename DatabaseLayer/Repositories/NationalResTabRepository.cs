using Dapper;
using DatabaseLayer.DBSettings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SQLite;
using System.Net.NetworkInformation;

namespace DatabaseLayer.Repositories
{
    public class NationalResTabRepository
    {
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
                    param: new { leagueId = leagueId, season = season },
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
                var record =
                    connection.QuerySingleOrDefault(
                        @$"SELECT * FROM NationalResultTable WHERE TeamId = @TeamId AND Season = @Season", model);

                if (record == null)
                    return result;

                var rowAffected = connection.Execute(
                     "UPDATE NationalResultTable SET "+
                        "Wins = @wins, Draws = @draws,"+
                        "Loses = @loses, ScoredGoals = @scoredGoals, MissedGoals = @missedGoals,"+
                        "TotalPosition = @totalPosition WHERE TeamId = @teamId AND Season = @season", 
                    param: new { wins = model.Wins, 
                        draws = model.Draws, 
                        loses = model.Loses, 
                        scoredGoals = model.ScoredGoals, 
                        missedGoals = model.MissedGoals, 
                        totalPosition = model.TotalPosition,
                        teamId = model.TeamID,
                        season = model.Season
                    });
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
                    param: new { teamId = teamId, season = season },
                    splitOn: "ID").AsList();
            }
            return result;
        }

        public bool Insert(NationalResultTable model)
        {
            bool result = false;
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();

                var record = connection.QueryFirstOrDefault(
                    "SELECT * FROM NationalResultTable WHERE TeamId = @teamId AND Season = @season",
                    param: new { teamId = model.TeamID, season = model.Season }
                );

                if (record != null)
                    return result;

                var rowAffected = connection.Execute(
                    @"INSERT INTO NationalResultTable (Season, TeamId, Wins, Draws, Loses, ScoredGoals, MissedGoals, TotalPosition)
                    VALUES (@Season, @TeamId, @Wins, @Draws, @Loses, @ScoredGoals, @MissedGoals, @TotalPosition)", model
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
    }
}

