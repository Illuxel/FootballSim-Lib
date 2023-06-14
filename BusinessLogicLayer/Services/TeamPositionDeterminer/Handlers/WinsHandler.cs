using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    internal class WinsHandler : PositionHandler
    {
        private string _season;
        public WinsHandler(string season)
        {
            _season = season;
        }
        public override void Handle(Dictionary<int, List<NationalResultTable>> team, List<int> teamsWithSamePositionsKeys)
        {
            foreach (var position in teamsWithSamePositionsKeys)
            {
                var numOfPosition = position;
                var teams = team[position];
                var sortedTeams = teams.OrderByDescending(x => x.Wins);

                team[numOfPosition] = new List<NationalResultTable>();
                foreach (var teamInPosition in sortedTeams)
                {
                    team[numOfPosition].Add(teamInPosition);
                    numOfPosition++;
                }
            }
            teamsWithSamePositionsKeys = SamePositions(team);
            if (nextHandler != null && teamsWithSamePositionsKeys.Count != 0)
            {
                nextHandler.Handle(team);
            }
            else
            {
                if (teamsWithSamePositionsKeys.Count == 0)
                {
                    saveData(team, _season);
                }
            }
        }
    }
}
