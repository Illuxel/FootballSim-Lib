using System.Collections.Generic;
using DatabaseLayer.DBSettings;
using System.Data.SQLite;
using Dapper;
using System;
using DatabaseLayer.Enums;
using System.Data;
using System.Linq;
using System.Transactions;

namespace DatabaseLayer.Repositories
{
    public class TeamRepository
    {
        public List<Team> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var sql = @"SELECT Team.*, League.*
                    FROM Team 
                    LEFT JOIN League on Team.LeagueID = League.ID
                    WHERE TEAM.RowState = @rowState";
                var results = connection.Query<Team, League, Team>(
                    sql,
                    (team, league) =>
                    {
                        team.League = league;
                        return team;
                    },
                    param: new { rowState = DbRowState.IsActive },
                    splitOn: "Id" 
                );
                return results.AsList();
            }
        }

        public List<Team> Retrieve(int leagueId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var sql = @"SELECT Team.*
                    FROM Team 
                    WHERE Team.LeagueID = @leagueId";
                var results = connection.Query<Team>(
                    sql,
                    param: new { leagueId }
                );
                return results.AsList();
            }
        }

        public Team Retrieve(string teamId)
        {
            if (string.IsNullOrEmpty(teamId))
            {
                return null;
            }
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var query = @"SELECT * FROM TEAM
                    WHERE ID = @teamId"
                ;

                var team = connection.QuerySingleOrDefault<Team>(query, new { teamId });

                team.League = connection.QuerySingleOrDefault<League>(
                    "SELECT * FROM LEAGUE WHERE LEAGUE.ID = @id", new { id = team.LeagueID });
                return team;
            }
        }

        internal bool Insert(Team team)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Team>("SELECT * FROM Team WHERE ID = @teamId", new { teamId = team.Id });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute(
                        @"INSERT INTO Team (ID, Name, LeagueID, Budget, SportDirectorID, CoachId, ScoutId, 
                            IsNationalTeam, Strategy, BaseColor, ExtId, ExtName, TacticSchema) 
                          VALUES (@Id, @Name, @LeagueId, @Budget, @SportDirectorId, @CoachId, @ScoutId, 
                            @IsNationalTeam, @Strategy, @BaseColor, @ExtId, @ExtName, @TacticSchema)", team);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }


        public bool Update(Team team)
        {

            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Team>("SELECT * FROM Team WHERE ID = @teamId", new { teamId = team.Id });
                bool result = false;
                if (record != null)
                {
                    Console.WriteLine(team.ExtName);
                    var rowsAffected = connection.Execute(
                        @"UPDATE Team SET 
                            Name = @Name, 
                            LeagueID = @LeagueId,
                            SportsDirectorID = @SportDirectorId,
                            Budget = @Budget,
                            CoachId = @CoachId,
                            ScoutId = @ScoutId, 
                            IsNationalTeam = @IsNationalTeam,
                            Strategy = @Strategy,
                            TacticSchema = @TacticSchema,
                            BaseColor = @BaseColor,
                            ExtId = @ExtId,
                            ExtName = @ExtName 
                            WHERE ID = @Id",
                        team);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        public bool Delete(string teamId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute("DELETE FROM Team WHERE Id = @teamId",
                    new { teamId });
                return rowsAffected == 1;
            }
        }


        public void UpdateRating(string teamId, double rating)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                connection.Execute(
                                @"UPDATE Team 
                                SET CurrentInterlRatingPosition = @rating
                                WHERE Id = @teamId",
                                new { rating, teamId });
            }
        }
        


        public void UpdateRating(List<Team> ratingPosition)
        {
            if (ratingPosition != null)
            {
                using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
                {
                    connection.Open();
                    using (IDbTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var rowsAffected = connection.Execute(
                            @"UPDATE Team 
                                SET CurrentInterlRatingPosition = @CurrentInterlRatingPosition
                                WHERE Id = @Id",
                                ratingPosition, transaction);
                            transaction.Commit();
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
}


