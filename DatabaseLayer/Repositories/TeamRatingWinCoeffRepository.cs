using Dapper;
using DatabaseLayer.DBSettings;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace DatabaseLayer.Repositories
{
    public class TeamRatingWinCoeffRepository
    {
        public List<TeamRatingWinCoeff> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {  
                return connection.Query<TeamRatingWinCoeff>("SELECT * FROM TeamRatingWinCoeff").AsList();
            }
        }

        public TeamRatingWinCoeff RetrieveOne(string teamId,string season)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                return connection.QueryFirstOrDefault<TeamRatingWinCoeff>(
                    @"SELECT * FROM TeamRatingWinCoeff 
                         WHERE TeamId = @teamId AND Season = @season",
                    new {teamId,season });
            }   
        }

        public List<string> RetrieveAllTeams()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                return connection.Query<string>("SELECT TeamId FROM TeamRatingWinCoeff").AsList().Distinct().ToList();
            }
        }
        public List<TeamRatingWinCoeff> RetrieveAllSeasonsByTeam(string teamId)
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
                if(response.Count>=5)
                {
                    return response.TakeLast(5).ToList();
                }
                return response;
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
        
       
    }
}
