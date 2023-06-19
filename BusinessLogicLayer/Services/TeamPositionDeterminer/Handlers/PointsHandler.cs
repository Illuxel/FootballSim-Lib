using DatabaseLayer;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    internal class PointsHandler : PositionHandlerBase
    {
        public PointsHandler(string season) : base(season)
        {
        }
        public override void Handle(Dictionary<int, List<NationalResultTable>> teams, List<int> keys = null)
        {
            var sortedResults = teams[0].OrderByDescending(x => x.TotalPoints).ToList();
            for (int i = 1; i <= sortedResults.Count; i++)
            {
                var teamsByPosition = sortedResults.Where(x => x.TotalPoints == sortedResults[i-1].TotalPoints).ToList();
                teams.Add(i, teamsByPosition);
                DeleteFromDictionary(teams, teamsByPosition);
                int count = teamsByPosition.Count;
                if (count > 1)
                {
                    var curIndex = i;
                    for (int j = curIndex + 1; j <= count + curIndex - 1; j++)
                    {
                        teams.Add(j, new List<NationalResultTable>());
                        if (j == count + i - 1)
                        {
                            i = j;
                        }
                    }
                }
            }
            var teamsWithSamePositionsKeys = SamePositions(teams);

            if (nextHandler != null && ArePositionsSet(teams) && teamsWithSamePositionsKeys.Count != 0)
            {
                nextHandler.Handle(teams, teamsWithSamePositionsKeys);
            }
            else if (ArePositionsSet(teams) && teamsWithSamePositionsKeys.Count == 0)
            {
                saveData(teams, _season);
            }
        }
    }
}
