﻿using System.Collections.Generic;
using DatabaseLayer.DBSettings;
using System.Data.SQLite;
using Dapper;
using System;
using DatabaseLayer.Enums;
using System.Data;
using DatabaseLayer.Model;

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
        
        public List<Team> Retrieve(List<string> teamsId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var teams = connection.Query<Team>(@"SELECT * FROM TEAM WHERE ID IN @teamsId", new { teamsId }).AsList();
                return teams;
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
        internal List<Player> RetrieveJuniors(string teamId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var sql = @"SELECT P.*, PR.*, PS.*
                    FROM PLAYER P
                    LEFT JOIN CONTRACT C on C.ID = P.CONTRACTID
                    INNER JOIN Person PR ON P.PersonID = PR.ID
                    LEFT JOIN Position PS ON P.PositionCode = Ps.Code
                    WHERE C.TEAMID = @teamId AND P.ISJUNIOR = 1 ";
                var results = connection.Query<Player, Person, Position, Player>(
                    sql,
                    (player, person, position) =>
                    {
                        player.Person = person;
                        player.Position = position;
                        return player;
                    },
                    param: new { teamId },
                    splitOn: "ID, CODE"
                );
                return results.AsList();
            }
        }

        public List<TeamSuccessHistory> GetHistory(string teamId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var response = connection.Query<TeamSuccessHistory>(@"SELECT NationalResultTable.TeamId, 
                    NationalResultTable.Season,
	                NationalResultTable.TotalPosition,
	                T.LeagueId
                    FROM NationalResultTable 
                    JOIN Team T on T.Id = NationalResultTable.TeamId
                    WHERE NationalResultTable.TeamId = @teamId", new { teamId }).AsList();
                return response;
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
                    var rowsAffected = connection.Execute(
                    @"UPDATE Team SET 
                        Name = @Name, 
                        LeagueID = @LeagueID,
                        SportsDirectorID = @SportsDirectorID,
                        CoachID = @CoachID,
                        ScoutID = @ScoutID, 
                        Budget = @Budget,
                        IsNationalTeam = @IsNationalTeam,
                        Strategy = @Strategy,
                        TacticSchema = @TacticSchema,
                        BaseColor = @BaseColor,
                        ExtId = @ExtId,
                        ExtName = @ExtName,
                        GlobalPosition = @GlobalPosition
                        WHERE Id = @Id",
                    team);

                    result = rowsAffected == 1;
                }
                return result;
            }
        }
        public bool Update(List<Team> teams)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var rowsAffected = connection.Execute(
                        @"UPDATE Team SET 
                            Name = @Name, 
                        LeagueID = @LeagueID,
                        SportsDirectorID = @SportsDirectorID,
                        CoachID = @CoachID,
                        ScoutID = @ScoutID, 
                        Budget = @Budget,
                        IsNationalTeam = @IsNationalTeam,
                        Strategy = @Strategy,
                        TacticSchema = @TacticSchema,
                        BaseColor = @BaseColor,
                        ExtId = @ExtId,
                        ExtName = @ExtName,
                        GlobalPosition = @GlobalPosition
                        WHERE Id = @Id",
                        teams,transaction);
                        transaction.Commit();
                        return rowsAffected != 0;
                    }
                    catch (Exception ex) 
                    {
                        throw new Exception(ex.Message);
                    }
                }
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
                                SET GlobalPosition = @GlobalPosition
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
                                SET
                                GlobalRating = @GlobalRating
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

        public bool UpdatePlayerRole(ITeamForMatch teamForMatch)
        {
            if(teamForMatch != null)
            {
                var players = teamForMatch.AllPlayers;
                using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
                {
                    connection.Open();
                    using (IDbTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var rowsAffected = connection.Execute(
                                @"UPDATE Player 
                                SET
                                PlayerPositionGroup = @PlayerPositionGroup
                                WHERE PersonID = @PersonID", players, transaction);
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }

            return false;
        }

        public bool UpdatePlayerRole(string personId, PlayerPositionGroup playerPositionGroup)
        {
            if (personId != null)
            {
                using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
                {
                    connection.Open();
                    var rowsAffected = connection.Execute(
                    @"UPDATE Player 
                    SET
                    PlayerPositionGroup = @playerPositionGroup
                    WHERE PersonID = @PersonID", new {playerPositionGroup, personId});
                    return rowsAffected == 1;
                }
            }

            return false;
        }
    }
}


