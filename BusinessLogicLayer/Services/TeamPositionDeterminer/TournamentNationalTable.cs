using DatabaseLayer;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    internal class TournamentNationalTable
    {
        Dictionary<int,List<NationalResultTable>> teams;
        PositionHandler positionHandler;

        public TournamentNationalTable(string season)
        {
            teams = new Dictionary<int, List<NationalResultTable>>();
    
            PositionHandler pointsHandler = new PointsHandler(season);
            PositionHandler goalDifferenceHandler = new GoalDifferenceHandler(season);
            PositionHandler headToHeadHandler = new HeadToHeadHandler(season);
            PositionHandler winsHandler = new WinsHandler(season);

            pointsHandler.SetNextHandler(goalDifferenceHandler);
            goalDifferenceHandler.SetNextHandler(headToHeadHandler);
            headToHeadHandler.SetNextHandler(winsHandler);

            positionHandler = pointsHandler;
        }
        public void AddTeam(NationalResultTable team)
        {
            if (teams.ContainsKey(0))
            {
                teams[0].Add(team);
            }
            else
            {
                teams.Add(0, new List<NationalResultTable>() { team });
            }
        }
        public void AddTeam(List<NationalResultTable> teamsList)
        {
            if (teams.ContainsKey(0))
            {
                teams[0] = teamsList;
            }
            else
            {
                teams.Add(0, teamsList);
            }
        }

        public void UpdatePositions()
        {
            positionHandler.Handle(teams);
        }
    }
}
