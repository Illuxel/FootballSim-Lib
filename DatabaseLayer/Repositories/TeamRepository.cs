using System.Collections.Generic;
using DatabaseLayer.DBSettings;
using System.Data.SQLite;
using Dapper;
using System;

namespace FootBalLife.Database.Repositories
{
    public class TeamRepository
    {
        public List<Team> Retrive()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var sql = @"SELECT Team.*, League.*
                    FROM Team 
                    LEFT JOIN League on Team.LeagueID = League.ID";
                var results = connection.Query<Team, League, Team>(
                    sql,
                    (team, league) =>
                    {
                        team.League = league;
                        return team;
                    }
                    //splitOn: "LeagueID" // Спричиняє помилку незаповненості таблиць
                );
                return results.AsList();
            }
        }

        public List<Team> Retrive(int leagueId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var sql = @"SELECT Team.*, League.*
                    FROM Team 
                    LEFT JOIN League on Team.LeagueID = League.ID
                    WHERE Team.LeagueID = @leagueId";
                var results = connection.Query<Team, League, Team>(
                    sql,
                    (team, league) =>
                    {
                        team.League = league;
                        return team;
                    },
                    param: new { leagueId }
                    //splitOn: "LeagueID"
                );
                return results.AsList();
            }
        }

        public Team Retrive(string teamId)
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
                        @"INSERT INTO Team (ID, Name, LeagueID, SportDirectorID, CoachId, ScoutId, IsNationalTeam, Strategy, BaseColor, ExtId, ExtName) 
                            VALUES (@Id, @Name, @LeagueId, @SportDirectorId, @CoachId, @ScoutId, @IsNationalTeam, @Strategy, @BaseColor, @ExtId, @ExtName)", team);
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
                            CoachId = @CoachId,
                            ScoutId = @ScoutId, 
                            IsNationalTeam = @IsNationalTeam,
                            Strategy = @Strategy,
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
    }
}


