using DatabaseLayer;
using DatabaseLayer.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    internal class HeadToHeadHandler : PositionHandlerBase
    {
        MatchRepository _matchRepository;
        SeasonValueCreator _seasonValueCreator;
        public HeadToHeadHandler(string season) : base(season)
        {
            _matchRepository = new MatchRepository();
            _seasonValueCreator = new SeasonValueCreator(); 
        }
        public override void Handle(Dictionary<int, List<NationalResultTable>> teams, List<int> teamsWithSamePositionsKeys)
        {
            var startDate = _seasonValueCreator.GetSeasonStartDate(_season);
            var endDate = _seasonValueCreator.GetSeasonEndDate(_season);
            foreach (var position in teamsWithSamePositionsKeys)
            {
                var teamsDict = new Dictionary<int, NationalResultTable>();
                var teamsByPosition = teams[position];

                foreach (var team1 in teamsByPosition)
                {
                    int points = 0;
                    var headByHeadMatches = new List<Match>();

                    foreach (var team2 in teamsByPosition)
                    {
                        if (team1 != team2)
                        {
                            headByHeadMatches = _matchRepository.Retrieve(team1.TeamID, team2.TeamID,startDate,endDate);

                            foreach (var match in headByHeadMatches)
                            {
                                if (match.HomeTeamId == team1.TeamID)
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
                                else if (match.GuestTeamId == team1.TeamID)
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
                            teamsDict.Add(points, team1);
                        }
                    }
                }

                var sortedTeams = teamsDict.OrderByDescending(x => x.Key).Select(x => x.Value).ToList();

                var numOfPosition = position;
                teams[numOfPosition] = new List<NationalResultTable>();

                foreach (var teamInPosition in sortedTeams)
                {
                    teams[numOfPosition].Add(teamInPosition);
                    numOfPosition++;
                }
            }

            teamsWithSamePositionsKeys = SamePositions(teams);

            if (nextHandler != null && teamsWithSamePositionsKeys.Count != 0)
            {
                nextHandler.Handle(teams);
            }
            else if (teamsWithSamePositionsKeys.Count == 0)
            {
                saveData(teams, _season);
            }
        }
    }
}
