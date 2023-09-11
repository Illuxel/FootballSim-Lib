using DatabaseLayer.Enums;
using DatabaseLayer.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseLayer.Services
{
    public class ScoutLeagueAccessor
    {
        TeamRepository _teamRepository;
        LeagueRepository _leagueRepository;

        private static int _LeagueRangeForSecondLevel = 2;
        private static int _amountOfLeaguesBelowForTheBestLeague = 4;
        public ScoutLeagueAccessor()
        {
            _teamRepository = new TeamRepository();
            _leagueRepository = new LeagueRepository();
        }

        public List<League> DefineAccessedLeagues(PlayerGameData playerGameData)
        {
            var team = defineTeam(playerGameData.ClubId);
            var accessedLeagues = defineLeagues(team, playerGameData);
            return accessedLeagues;
        }

        private Team defineTeam(string teamId)
        {
            return _teamRepository.Retrieve(teamId);
        }

        private List<League> defineLeagues(Team team, PlayerGameData playerGameData)
        {
            var scoutLeague = team.League;
            if (playerGameData.CurrentLevel == ScoutSkillLevel.Level1)
            {
                return new List<League>() { scoutLeague };
            }
            else if (playerGameData.CurrentLevel == ScoutSkillLevel.Level2)
            {
                var accessedLeagues = new List<League>();
                var allLeagues = _leagueRepository.Retrieve();
                var sortedLeagues = allLeagues.OrderBy(x => x.CurrentRating).ToList();

                int maxIndex = sortedLeagues.Count - 1;
                int scoutLeagueIndex = sortedLeagues.FindIndex(x => x.Id == scoutLeague.Id);
                if(scoutLeagueIndex == maxIndex)
                {
                    for (int i = maxIndex - _amountOfLeaguesBelowForTheBestLeague; i<=maxIndex;i++)
                    {
                        accessedLeagues.Add(sortedLeagues[i]);
                    }
                    return accessedLeagues;
                }
                int lastLeftNeighbourIndex = scoutLeagueIndex - _LeagueRangeForSecondLevel;
                int lastRightNeighbourIndex = scoutLeagueIndex + _LeagueRangeForSecondLevel;

                for (int i = lastLeftNeighbourIndex; i <= lastRightNeighbourIndex; i++)
                {
                    if (i >= 0 && i <= maxIndex)
                    {
                        accessedLeagues.Add(sortedLeagues[i]);
                    }
                }
                return accessedLeagues;

            }
            return _leagueRepository.Retrieve();
        }
    }
}
