using DatabaseLayer.Enums;
using DatabaseLayer.Model;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;

namespace DatabaseLayer.Services
{
    public class JuniorPlayerGetter
    {
        AccessedLeagues _accessedLeagues;
        TeamRepository _teamRepository;
        JuniorFinder _juniorFinder;

        public JuniorPlayerGetter()
        {
            _accessedLeagues = new AccessedLeagues();
            _teamRepository = new TeamRepository();
            _juniorFinder = new JuniorFinder();
        }
        public JuniorTeam Get(string teamId, ref PlayerGameData playerGameData)
        {
            var allJuniorsTeams = getAllJuniorsTeams(playerGameData);
            var leagueId = _teamRepository.Retrieve(teamId).League.Id;
            
            allJuniorsTeams.TryGetValue(leagueId, out var juniorTeamsInLeague);
            if(juniorTeamsInLeague != null && playerGameData.CountAvailableScoutRequests != 0)
            {
                playerGameData.CountAvailableScoutRequests -= 1;
                var juniorTeam = juniorTeamsInLeague.Find(x => x.TeamId == teamId);
                return juniorTeam;
            }
            return null;
        }

        private Dictionary<int, List<JuniorTeam>> getAllJuniorsTeams(PlayerGameData playerGameData)
        {
            var juniorTeams = new Dictionary<int, List<JuniorTeam>>();
            var gameDate = DateTime.Parse(playerGameData.GameDate);
            var leagues = _accessedLeagues.DefineAccessedLeagues(playerGameData);

            foreach (var league in leagues)
            {
               var juniorTeamsInLeague = new List<JuniorTeam>();
               var teams = _teamRepository.Retrieve(league.Id);

               foreach (var team in teams)
               {
                    var juniorTeam = new JuniorTeam();

                    juniorTeam.TeamId = team.Id;
                    if(playerGameData.CurrentLevel == ScoutSkillLevel.Level2)
                    {
                        juniorTeam.AverageTeamRating = _juniorFinder.AverageJuniorRatingByTeam(juniorTeam.TeamId);
                    }
                    else if(playerGameData.CurrentLevel == ScoutSkillLevel.Level3)
                    {
                        juniorTeam.AverageTeamRating = _juniorFinder.AverageJuniorRatingByTeam(juniorTeam.TeamId);
                        juniorTeam.BestJunior = _juniorFinder.BestJuniorPlayerByTeam(juniorTeam.TeamId,gameDate);
                    }

                    juniorTeamsInLeague.Add(juniorTeam);
               }
               juniorTeams.Add(league.Id, juniorTeamsInLeague);
            }

            return juniorTeams;
        }
    }
}
