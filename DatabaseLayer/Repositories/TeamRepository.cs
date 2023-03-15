using System.Collections.Generic;
using DatabaseLayer.DBSettings;
using System.Data.SQLite;
using Dapper;
using System.Linq;

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
                    },
                    splitOn: "LeagueID"
                );
                return results.AsList();
            }
        }
        public Team Retrive(string teamId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var query = @"SELECT Team.*, Contract.*, League.*
                    FROM Team
                    LEFT JOIN Contract ON Contract.TeamID = Team.ID
                    LEFT JOIN League on Team.LeagueID = League.ID
                    WHERE Team.ID = @teamId"
                ;

                using (var multi = connection.QueryMultiple(query, new { teamId }))
                {
                    var team = multi.Read<Team>().FirstOrDefault();
                    var contracts = multi.Read<Contract>();
                    var league = multi.Read<League>().FirstOrDefault();
                    //team.Contracts = contracts.ToList();
                    team.League = league;

                    return team;
                }
            }
        }

        internal bool Insert(Team team)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Role>("SELECT * FROM Team WHERE ID = @teamId", new { teamId = team.Id });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute(
                        @"INSERT INTO Team (ID, Name, LeagueID, SportDirectorID, CoachId, ScoutId, IsNationalTeam, Strategy, BaseColor) 
                            VALUES (@Id, @Name, @LeagueId, @SportDirectorId, @CoachId, @ScoutId, @IsNationalTeam, @Strategy, @BaseColor)", team);
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
                var record = connection.QuerySingleOrDefault<Role>("SELECT * FROM Team WHERE ID = @teamId", new { teamId = team.Id });
                bool result = false;
                if (record != null)
                {
                    var rowsAffected = connection.Execute(
                        @"UPDATE Team SET 
                            Name = @Name, 
                            LeagueID = @LeagueId,
                            SportDirectorID = @SportDirectorId,
                            CoachId = @CoachId,
                            ScoutId = @ScoutId, 
                            IsNationalTeam = @IsNationalTeam,
                            Strategy = @Strategy,
                            BaseColor = @BaseColor
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


