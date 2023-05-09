using Dapper;
using DatabaseLayer.DBSettings;
using System.Collections.Generic;
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
    }
}

