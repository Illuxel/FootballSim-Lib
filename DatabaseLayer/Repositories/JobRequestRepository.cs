using System;
using System.Linq;
using System.Data.SQLite;
using System.Collections.Generic;

using Dapper;
using DatabaseLayer.DBSettings;

namespace DatabaseLayer.Repositories
{
    public class JobRequestRepository
    {
        public List<JobRequest> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var jobRequests = connection.Query<JobRequest>(
                    @"SELECT * FROM JobRequest WHERE 1 = 1");

                return jobRequests.AsList();
            }
        }
        public JobRequest Retrieve(string jobRequestID)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var jobRequests = connection.QuerySingle<JobRequest>(
                    @"  SELECT * FROM JobRequest 
                        WHERE ID = @jobRequestID", 
                        new { jobRequestID = jobRequestID });

                return jobRequests;
            }
        }
        public List<JobRequest> RetrieveByPerson(string personID)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var jobRequests = connection.Query<JobRequest>(
                    @"  SELECT * FROM JobRequest 
                        WHERE PersonID = @personID", 
                    new { PersonID = personID });

                return jobRequests.AsList();
            }
        }
        // returns available job requests for each player
        public Dictionary<string, List<JobRequest>> RetrieveByTeam(string teamID)
        {
            var playersJobRequests = new Dictionary<string, List<JobRequest>>();
            var teamRepo = new TeamRepository();
            var team = teamRepo.Retrieve(teamID);

            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                team.Players.ForEach(p =>
                {
                    var playerJobRequests = connection.Query<JobRequest>(
                        @"  SELECT * FROM JobRequest 
                            WHERE PersonID = @personID AND TeamID = @teamID", 
                            new { 
                                personID = p.PersonID,
                                teamID = teamID,
                            });

                    if (playerJobRequests.Count() != 0)
                    {
                        playersJobRequests.Add(p.PersonID, playerJobRequests.AsList());
                    }
                });
            }

            return playersJobRequests;
        }

        public bool Insert(JobRequest jobRequest)
        {
            jobRequest.ID = Guid.NewGuid().ToString(); 

            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var result = connection.Execute(
                    @"  INSERT INTO JobRequest(ID, PersonID, TeamID, Salary, DurationTo) 
                        VALUES(@ID, @PersonID, @TeamID, @Salary, @DurationTo)", 
                        jobRequest
                );

                return result != 0;
            }
        }
        public bool Insert(List<JobRequest> jobRequests)
        {
            jobRequests.ForEach(jobRequest => jobRequest.ID = Guid.NewGuid().ToString());

            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try 
                    {
                        var rowsAffected = connection.Execute(
                            @"  INSERT INTO JobRequest(ID, PersonID, TeamID, Salary, DurationTo) 
                                VALUES(@ID, @PersonID, @TeamID, @Salary, @DurationTo)", 
                                jobRequests,
                                transaction
                        );
                        transaction.Commit();
                        return rowsAffected != 0;
                    }
                    catch (SQLiteException e)
                    {
                        transaction.Rollback();
                        throw e;
                    }       
                }
            }
        }
    }
}