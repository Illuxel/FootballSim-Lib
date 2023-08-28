using System.Data.SQLite;
using System.Collections.Generic;

using DatabaseLayer.DBSettings;

namespace DatabaseLayer.Repositories
{
    public class JobRequestRepository
    {
        public List<JobRequest> Retrieve(string matchId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();

                return new List<JobRequest>();
            }
        }
        public bool Insert(JobRequest jobRequest)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();

                return true;
            }
        }
        public bool Insert(List<JobRequest> jobRequests)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();

                return true;
            }
        }

    }
}