using DatabaseLayer;
using DatabaseLayer.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class TeamPositionCalculator
    {
        NationalResTabRepository _nationalResTabRepository;
        TeamRepository _teamRepository;

        public TeamPositionCalculator()
        {
            _nationalResTabRepository = new NationalResTabRepository();
            _teamRepository = new TeamRepository();
        }

        public void CalculatePosition(string season)
        {
            var results = getAllResults(season);
            var getAllTeams = getTeamsByLeague();
        }

        private Dictionary<string, NationalResultTable> getAllResults(string season)
        {
            return _nationalResTabRepository.Retrieve(season);
        }
        private Dictionary<int, List<string>> getTeamsByLeague()
        {
            var teams = _teamRepository.Retrieve();
            var teamsByLeague = teams.GroupBy(t => t.LeagueID).ToDictionary(t => t.Key, t => t.Select(x => x.Id).ToList());
            return teamsByLeague;
        }
        private Dictionary<int, List<NationalResultTable>> resultsByLeague(Dictionary<int, List<string>> teamsByLeague, Dictionary<string, NationalResultTable> resultByTeam)
        {
            var resultsByLeague = new Dictionary<int, List<NationalResultTable>>();

            foreach (var kvp in teamsByLeague)
            {
                var results = new List<NationalResultTable>(kvp.Value.Count);

                foreach (var teamID in kvp.Value)
                {
                    if (resultByTeam.TryGetValue(teamID, out var result))
                    {
                        results.Add(result);
                    }
                }

                resultsByLeague[kvp.Key] = results;
            }

            return resultsByLeague;
        }

       /* private List<NationalResultTable> calculatePositionByLeague(Dictionary<int, List<NationalResultTable>> resultsByLeague)
        {

        }*/

        private List<NationalResultTable> firstCritery(List<NationalResultTable> results)
        {
            return results.OrderByDescending(x => x.TotalPoints).ToList();
        }
        private List<NationalResultTable> secondCritery(List<NationalResultTable> results)
        {
            return results.OrderByDescending(x => x.ScoredGoals - x.MissedGoals).ToList();
        }
        /*private List<NationalResultTable> thirdCritery(List<NationalResultTable> results)
        {

        }
        private List<NationalResultTable> fourthCritery(List<NationalResultTable> results);*/
        private List<List<NationalResultTable>> findTeamsWithEqualPoints(List<NationalResultTable> results)
        {
            var teamsWithEqualPoints = results.GroupBy(x => x.TotalPoints)
                                              .Where(g => g.Count() > 1)
                                              .Select(g => g.ToList())
                                              .ToList();

            return teamsWithEqualPoints;
        }
    }
}
