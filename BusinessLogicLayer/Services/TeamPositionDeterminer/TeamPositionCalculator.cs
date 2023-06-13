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
        MatchRepository _matchRepository;

        public TeamPositionCalculator()
        {
            _nationalResTabRepository = new NationalResTabRepository();
            _teamRepository = new TeamRepository();
            _matchRepository = new MatchRepository();
        }

        public void CalculatePosition(string season)
        {
            var getAllTeams = getTeamsByLeague();

            foreach (var kvp in getAllTeams)
            {
                var teamIDs = kvp.Value;
                var results = getResultsByLeague(season, teamIDs);

                var currentResults = firstCritery(results);
                int critery = 1;

                while (findTeamsWithEqualPoints(currentResults) != null)
                {
                    critery++;

                    if (critery == 2)
                    {
                        currentResults = secondCritery(currentResults);
                    }
                    else if (critery == 3)
                    {
                        currentResults = thirdCritery(currentResults);
                    }
                    else if (critery == 4)
                    {
                        currentResults = fourthCritery(currentResults);
                    }
                }

                assignPositions(currentResults);
            }
        }

        private Dictionary<int, List<string>> getTeamsByLeague()
        {
            var teams = _teamRepository.Retrieve();
            return teams.GroupBy(t => t.LeagueID).ToDictionary(t => t.Key, t => t.Select(x => x.Id).ToList());
        }

        private List<NationalResultTable> getResultsByLeague(string season, List<string> teamIDs)
        {
            var results = _nationalResTabRepository.Retrieve(season);
            return teamIDs.Select(teamID => results.TryGetValue(teamID, out var result) ? result : null).ToList();
        }

        private List<NationalResultTable> firstCritery(List<NationalResultTable> results)
        {
            return results.OrderByDescending(x => x.TotalPoints).ToList();
        }

        private List<NationalResultTable> secondCritery(List<NationalResultTable> results)
        {
            return results.OrderByDescending(x => x.ScoredGoals - x.MissedGoals).ToList();
        }

        private List<NationalResultTable> thirdCritery(List<NationalResultTable> results)
        {
            var resultsWithPoints = new List<(NationalResultTable, int)>();

            foreach (var res1 in results)
            {
                int points = 0;

                foreach (var res2 in results)
                {
                    if (res1 != res2)
                    {
                        var matches = _matchRepository.Retrieve(res1.TeamID, res2.TeamID);

                        foreach (var match in matches)
                        {
                            if (match.HomeTeamId == res1.TeamID)
                            {
                                if (match.HomeTeamGoals > match.GuestTeamGoals)
                                {
                                    points += 3;
                                }
                                else if (match.HomeTeamGoals == match.GuestTeamGoals)
                                {
                                    points += 1;
                                }
                            }
                            else if (match.GuestTeamId == res1.TeamID)
                            {
                                if (match.GuestTeamGoals > match.HomeTeamGoals)
                                {
                                    points += 3;
                                }
                                else if (match.HomeTeamGoals == match.GuestTeamGoals)
                                {
                                    points += 1;
                                }
                            }
                        }
                    }
                }

                resultsWithPoints.Add((res1, points));
            }

            resultsWithPoints.Sort((x, y) => y.Item2.CompareTo(x.Item2));
            return resultsWithPoints.Select(x => x.Item1).ToList();
        }

        private List<NationalResultTable> fourthCritery(List<NationalResultTable> results)
        {
            return results.OrderByDescending(x => x.Wins).ToList();
        }

        private Dictionary<int, List<NationalResultTable>> findTeamsWithEqualPoints(List<NationalResultTable> results)
        {
            return results.GroupBy(x => x.TotalPoints).Where(g => g.Count() > 1).ToLookup(g => g.Key, g => g.ToList()).ToDictionary(g => g.Key, g => g.FirstOrDefault());
        }

        private void assignPositions(List<NationalResultTable> results)
        {
            int position = 1;

            for (int i = 0; i < results.Count; i++)
            {
                results[i].TotalPosition = position;
                position++;
            }
        }
    }

}
