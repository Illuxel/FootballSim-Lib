using DatabaseLayer;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    internal class PointsHandler : PositionHandler
    {
        private string _season;
        public PointsHandler(string season)
        {

            _season = season;

        }
        public override void Handle(Dictionary<int, List<NationalResultTable>> team, List<int> keys = null)
        {
            var sortedResults = team[0].OrderByDescending(x => x.TotalPoints).ToList();
            for (int i = 0; i < sortedResults.Count; i++)
            {
                var teamsByPosition = sortedResults.Where(x => x.TotalPoints == sortedResults[i].TotalPoints);
                team.Add(i, teamsByPosition.ToList());
                var count = teamsByPosition.Count();
                if (count != 1)
                {
                    for (int j = i; j < i + count; j++)
                    {
                        team.Add(j, new List<NationalResultTable>());
                        i++;
                    }
                }
            }
            var teamsWithSamePositionsKeys = SamePositions(team);

            if (nextHandler != null && ArePositionsSet(team) && teamsWithSamePositionsKeys.Count != 0)
            {
                nextHandler.Handle(team, teamsWithSamePositionsKeys);
            }
            else
            {
                if(ArePositionsSet(team) && teamsWithSamePositionsKeys.Count == 0)
                {
                    saveData(team, _season);
                }
            }
        }
    }
}
