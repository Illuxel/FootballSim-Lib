using DatabaseLayer;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    internal class TournamentNationalTable
    {
        Dictionary<int,List<NationalResultTable>> _teams;
        PositionHandlerBase positionHandler;

        public TournamentNationalTable(string season)
        {
            _teams = new Dictionary<int, List<NationalResultTable>>();
    
            PositionHandlerBase pointsHandler = new PointsHandler(season);
            PositionHandlerBase goalDifferenceHandler = new GoalDifferenceHandler(season);
            PositionHandlerBase headToHeadHandler = new HeadToHeadHandler(season);
            PositionHandlerBase winsHandler = new WinsHandler(season);

            pointsHandler.SetNextHandler(goalDifferenceHandler);
            goalDifferenceHandler.SetNextHandler(headToHeadHandler);
            headToHeadHandler.SetNextHandler(winsHandler);

            positionHandler = pointsHandler;
        }
        public void AddTeam(NationalResultTable team)
        {
            if (!_teams.TryGetValue(0, out List<NationalResultTable> existsTeams))
            {
                existsTeams = new List<NationalResultTable>();
                _teams[0] = existsTeams;
            }
            existsTeams.Add(team);
        }

        public void AddTeams(List<NationalResultTable> teamsList)
        {
            if (!_teams.TryGetValue(0, out List<NationalResultTable> existsTeams))
            {
                existsTeams = new List<NationalResultTable>();
                _teams[0] = existsTeams;
            }
            existsTeams.AddRange(teamsList);
        }
        public void UpdatePositions()
        {
            positionHandler.Handle(_teams);
        }
    }
}
