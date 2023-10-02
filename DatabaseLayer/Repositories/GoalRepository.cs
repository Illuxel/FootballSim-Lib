using Dapper;
using DatabaseLayer.DBSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace DatabaseLayer.Repositories
{
	public class GoalRepository
	{
		public List<Goal> Retrieve(string matchId)
		{
			using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
			{
				connection.Open();
				var goals = connection.Query<Goal>("SELECT * FROM Goal WHERE MatchId = @matchId", matchId).AsList();

				return goals;
			}
		}

        public List<PlayerStatistic> GetPlayerStatistic(string playerId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var playerStatistics = connection.Query<PlayerStatistic>(@"
				SELECT
                C.PersonID as PlayerId,
                C.TeamID,
                C.SeasonFrom,
                C.SeasonTo,
                COUNT(G.Id) AS CountOfGoals,
                COUNT(A.Id) AS CountOfAssists,
                COUNT(PIM.MatchId) AS CountOfPlayedMatches
				FROM Contract C
				JOIN Match M ON M.MatchDate BETWEEN DATE(C.DateFrom) AND DATE(C.DateTo) AND (M.GuestTeamId = C.TeamId OR M.HomeTeamId = C.TeamId)
				JOIN PlayerInMatch PIM ON PIM.PlayerId = C.PersonID AND PIM.MatchId = M.Id
				LEFT JOIN Goal G ON G.PlayerId = C.PersonID AND G.TeamId = C.TeamId AND G.MatchId = M.Id
				LEFT JOIN Goal A ON A.AssistPlayerId = C.PersonID AND A.TeamId = C.TeamId AND A.MatchId = M.Id
				WHERE C.PersonID = @playerId
				GROUP BY C.PersonID, C.TeamID, C.SeasonFrom, C.SeasonTo", new { playerId }).AsList();

                var playerRepository = new PlayerRepository();
				var player = playerRepository.RetrieveOne(playerId);
				foreach(var obj in playerStatistics)
				{
					obj.Player = player;
				}

                return playerStatistics;
            }
        }


        public List<PlayerStatistic> GetTopGoalScorers(int leagueId, string season, DateTime seasonStartDate, DateTime seasonEndDate, int limit = 10)
		{
			using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
			{
				connection.Open();
				var playerStatistics = connection.Query<PlayerStatistic>(@"
				SELECT
				G.PlayerId,
				G.TeamId,
				COUNT(G.PlayerId) as CountOfGoals,
				0 as CountOfAssists,
				(
					SELECT COUNT(*)
					FROM PlayerInMatch PIM
					WHERE PIM.PlayerId = G.PlayerId
				) AS CountOfPlayedMatches,
				@season AS Season
				FROM
					Goal G
				JOIN
					Match M ON G.MatchId = M.Id
				WHERE
					M.LeagueId = @leagueId AND M.MatchDate BETWEEN Date(@seasonStartDate) AND Date(@seasonEndDate) AND G.PlayerId NOT IN ('00000000-0000-0000-0000-000000000000', '')
				GROUP BY
					G.PlayerId
				ORDER BY
					CountOfGoals DESC,
					CountOfPlayedMatches ASC
				LIMIT @limit;", 
				new { leagueId, seasonStartDate, seasonEndDate, limit, season });

				var playerRepository = new PlayerRepository();
				var players = playerRepository.Retrieve(playerStatistics.Select(p=>p.PlayerId).ToList());

				foreach (var playerStat in playerStatistics)
				{
					if(players.TryGetValue(playerStat.PlayerId, out Player player))
					{
						playerStat.Player = player;
					}
				}

				return playerStatistics.ToList();
			}
		}
		public List<PlayerStatistic> GetTopAssistents(int leagueId, string season, DateTime seasonStartDate, DateTime seasonEndDate, int limit = 10)
		{
			using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
			{
				connection.Open();
				var playerStatistics = connection.Query<PlayerStatistic>(@"
				SELECT
				G.AssistPlayerId as PlayerId,
				G.TeamId,
				0 as CountOfGoals,
				COUNT(G.AssistPlayerId) as CountOfAssists,
				(
					SELECT COUNT(*)
					FROM PlayerInMatch PIM
					WHERE PIM.PlayerId = G.AssistPlayerId
				) AS CountOfPlayedMatches,
				@season AS Season
				FROM
					Goal G
				JOIN
					Match M ON G.MatchId = M.Id
				WHERE
					M.LeagueId = @leagueId AND M.MatchDate BETWEEN Date(@seasonStartDate) AND Date(@seasonEndDate) AND AssistPlayerId NOT IN ('00000000-0000-0000-0000-000000000000', '')
				GROUP BY
					G.AssistPlayerId
				ORDER BY
					CountOfAssists DESC,
					CountOfPlayedMatches ASC
				LIMIT @limit;", 
				new { leagueId, seasonStartDate, seasonEndDate,  limit, season });

				var playerRepository = new PlayerRepository();
				var players = playerRepository.Retrieve(playerStatistics.Select(p => p.PlayerId).ToList());


				foreach (var playerStat in playerStatistics)
				{
					if (players.TryGetValue(playerStat.PlayerId, out Player player))
					{
						playerStat.Player = player;
					}
				}

				return playerStatistics.ToList();
			}
		}

		public bool Insert(Goal goal)
		{
			using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
			{
				connection.Open();
				goal.Id = Guid.NewGuid().ToString();
				var rowsAffected = connection.Execute(
						@"INSERT INTO Goal (Id, MatchId, PlayerId, TeamId, MatchMinute, AssistPlayerId)
						VALUES (@Id, @MatchId, @PlayerId, @TeamId, @MatchMinute, @AssistPlayerId)",
						goal);
				 var result = rowsAffected == 1;
				
				return result;
			}
		}
		public bool Insert(List<Goal> goals)
		{
			using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
			{
				connection.Open();
				using (IDbTransaction transaction = connection.BeginTransaction())
				{
					try
					{
						var rowsAffected = connection.Execute(
							@"INSERT INTO Goal (Id, MatchId, PlayerId, TeamId, MatchMinute, AssistPlayerId)
						VALUES (@Id, @MatchId, @PlayerId, @TeamId, @MatchMinute, @AssistPlayerId)",
							goals);
						var result = rowsAffected == 1;

						transaction.Commit();
						return result;
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
