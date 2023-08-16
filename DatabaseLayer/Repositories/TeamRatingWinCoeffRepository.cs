using Dapper;
using DatabaseLayer.DBSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace DatabaseLayer.Repositories
{
    public class TeamRatingWinCoeffRepository
    {
        public Dictionary<string, List<TeamRatingWinCoeff>> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();

                var response = connection.Query<TeamRatingWinCoeff>("SELECT * FROM TeamRatingWinCoeff").AsList();

                Dictionary<string, List<TeamRatingWinCoeff>> dict =new Dictionary<string, List<TeamRatingWinCoeff>>();
                foreach (var ratingCoeff in response)
                {
                    if (!dict.TryGetValue(ratingCoeff.TeamId, out var list))
                    {
                        list = new List<TeamRatingWinCoeff>();
                        dict[ratingCoeff.TeamId] = list;
                    }
                    list.Add(ratingCoeff);
                }
                return dict;
            }
        }
        public List<TeamRatingWinCoeff> Retrieve(string teamId)
        {
            if (string.IsNullOrEmpty(teamId))
            {
                return null;
            }
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var response = connection.Query<TeamRatingWinCoeff>(
                    @"SELECT * FROM TeamRatingWinCoeff
                    WHERE TeamId = @teamId", new { teamId }).AsList();
                return response;
            }
        }
        public TeamRatingWinCoeff Retrieve(string teamId, string season)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<TeamRatingWinCoeff>(
                    @"SELECT * FROM TeamRatingWinCoeff 
                         WHERE TeamId = @teamId AND Season = @season",
                    new { teamId, season });
            }
        }
        public List<TeamRatingWinCoeff> Retrieve(string teamId,List<string> seasons)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                return connection.Query<TeamRatingWinCoeff>(
                    @"SELECT * FROM TeamRatingWinCoeff
                    WHERE TeamId = @teamId AND Season in @seasons",
                    new {teamId,seasons}).AsList();
            }
        }

        /// <summary>
        /// Get teams rating by seasons
        /// </summary>
        /// <param name="seasons"></param>
        /// <returns>Dictionary key - teamId, value - seasons rating </returns>
        public Dictionary<string, List<TeamRatingWinCoeff>> Retrieve(List<string> seasons)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var response = connection.Query<TeamRatingWinCoeff>(
                    @"SELECT * FROM TeamRatingWinCoeff
                    WHERE Season in @seasons",
                     new { Seasons = seasons.ToArray() }).AsList();
                Dictionary<string, List<TeamRatingWinCoeff>> dict = new Dictionary<string, List<TeamRatingWinCoeff>>();
                foreach (var ratingCoeff in response)
                {
                    if (!dict.TryGetValue(ratingCoeff.TeamId, out var list))
                    {
                        list = new List<TeamRatingWinCoeff>();
                        dict[ratingCoeff.TeamId] = list;
                    }
                    list.Add(ratingCoeff);
                }
                return dict;
            }
        }
        

        public bool Insert(TeamRatingWinCoeff teamCoeff)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                
                var record = connection.QueryFirstOrDefault<TeamRatingWinCoeff>(
                    @"SELECT * FROM TeamRatingWinCoeff 
                        WHERE TeamId = @teamId AND Season = @season", 
                    teamCoeff);
                
                bool result;
                if (record == null)
                {
                    var rowsAffected = connection.Execute(
                        @"INSERT INTO TeamRatingWinCoeff (TeamId, Season, WinCoeff) 
                          VALUES (@TeamId, @Season, @WinCoeff)", 
                        teamCoeff);
                    
                    result = rowsAffected == 1;
                }
                else
                {
                    Update(teamCoeff);
                    return false;
                }
                return result;
            }
        }


        public bool Update(TeamRatingWinCoeff teamCoeff)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();

                var rowsAffected = connection.Execute(
                        @"UPDATE TeamRatingWinCoeff SET 
                            WinCoeff = @WinCoeff
                            WHERE TeamId = @TeamId AND Season = @Season",
                        teamCoeff);
                return rowsAffected == 1;
            }
        }
        public bool Update(List<TeamRatingWinCoeff> teamCoeffList)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var rowsAffected = connection.Execute(
                            @"UPDATE TeamRatingWinCoeff SET 
                            WinCoeff = @WinCoeff
                            WHERE TeamId = @TeamId AND Season = @Season",
                            teamCoeffList);
                        transaction.Commit();
                        return rowsAffected == 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public bool InsertNewTeams(List<string> teamIds, string season)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();

                var existingTeams = connection.Query<string>(
                    @"SELECT TeamId FROM TeamRatingWinCoeff WHERE Season = @season",
                    new { season });

                var teamsToInsert = teamIds.Except(existingTeams).ToList();

                if (teamsToInsert.Any())
                {
                    using (IDbTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string insertQuery = @"INSERT INTO TeamRatingWinCoeff (TeamId, Season, WinCoeff) 
                              VALUES (@TeamId, @Season, @WinCoeff)";

                            List<TeamRatingWinCoeff> collection = teamsToInsert.Select(teamId => new TeamRatingWinCoeff
                            {
                                TeamId = teamId,
                                Season = season,
                                WinCoeff = 0
                            }).ToList();

                            var rowsAffected = connection.Execute(insertQuery, collection, transaction);

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                        return true;
                    }

                }
                return false;
            }
        }


    }
}
