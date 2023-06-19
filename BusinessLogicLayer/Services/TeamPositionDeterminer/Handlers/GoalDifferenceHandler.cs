using DatabaseLayer;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    internal class GoalDifferenceHandler : PositionHandlerBase
    {
        public GoalDifferenceHandler(string season) : base(season)
        {
        }
        public override void Handle(Dictionary<int, List<NationalResultTable>> teams, List<int> teamsWithSamePositionsKeys)
        {
            foreach (var position in teamsWithSamePositionsKeys)
            {
                var numOfPosition = position;
                var team = teams[position];
                var sortedTeams = team.OrderByDescending(x => x.ScoredGoals - x.MissedGoals);

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
                nextHandler.Handle(teams, teamsWithSamePositionsKeys);
            }
            else
            {
                if(teamsWithSamePositionsKeys.Count == 0)
                {
                    saveData(teams, _season);
                }
            }
        }
    }
}
