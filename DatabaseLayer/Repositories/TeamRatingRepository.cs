using Dapper;
using DatabaseLayer.DBSettings;
using DatabaseLayer.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace DatabaseLayer.Repositories
{
    internal class TeamRatingRepository
    {
       public List<TeamRating> Retrive()
       {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                return connection.Query<TeamRating>("SELECT * FROM TeamRating").AsList();
            }
       }
       public TeamRating RetriveOne(string teamId)
       {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<TeamRating>($"SELECT * FROM TeamRating WHERE TeamId = {teamId}");
            }
       }
        public bool Insert(TeamRating _team)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var query = "INSERT INTO TeamRating (TeamId, CurrentPosition, PercentPerSeason1, PercentPerSeason2, PercentPerSeason3, PercentPerSeason4, PercentPerSeason5) VALUES (@TeamId, @CurrentPosition, @PercentPerSeason1, @PercentPerSeason2, @PercentPerSeason3, @PercentPerSeason4, @PercentPerSeason5)";
                var parameters = new
                {
                    TeamId = _team.TeamId,
                    CurrentPosition = _team.CurrentPosition,
                    PercentPerSeason1 = _team.PercentPerSeason1,
                    PercentPerSeason2 = _team.PercentPerSeason2,
                    PercentPerSeason3 = _team.PercentPerSeason3,
                    PercentPerSeason4 = _team.PercentPerSeason4,
                    PercentPerSeason5 = _team.PercentPerSeason5
                };
                var rowsAffected = connection.Execute(query, parameters);
                return rowsAffected == 1;
            }
        }
        public bool Update(TeamRating _team)
        {

            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var record = connection.QuerySingleOrDefault<Team>("SELECT * FROM Team WHERE ID = @teamId", new { teamId = _team.TeamId});
                bool result = false;
                if (record != null)
                {
                    var rowsAffected = connection.Execute(
                        @"UPDATE TeamRating SET 
                            TeamId = @_team.TeamId,
                            CurrentPosition = @_team.CurrentPosition,
                            PercentPerSeason1 = @_team.PercentPerSeason1,
                            PercentPerSeason2 = @_team.PercentPerSeason2,
                            PercentPerSeason3 = @_team.PercentPerSeason3,
                            PercentPerSeason4 = @_team.PercentPerSeason4,
                            PercentPerSeason5 = @_team.PercentPerSeason5
                            WHERE TeamId = @_team.TeamId",
                        _team);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }
    }
}
