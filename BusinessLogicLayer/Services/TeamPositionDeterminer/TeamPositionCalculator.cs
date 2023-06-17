using DatabaseLayer;
using DatabaseLayer.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class TeamPositionCalculator
    {
        TeamRepository _teamRepository;
        TournamentNationalTable _tournamentNationalTable;
        NationalResTabRepository _nationalResTabRepository;
        public TeamPositionCalculator(string season)
        {
            _teamRepository = new TeamRepository();
            _nationalResTabRepository = new NationalResTabRepository();
        }

        public void CalculatePosition(string season)
        {
            var getAllTeams = getTeamsByLeague();
            foreach (var league in getAllTeams)
            {
                _tournamentNationalTable = new TournamentNationalTable(season);
                var teamIDs = league.Value;
                var results = getResultsByLeague(season, teamIDs);

                _tournamentNationalTable.AddTeam(results);
                _tournamentNationalTable.UpdatePositions();
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
    }

}
