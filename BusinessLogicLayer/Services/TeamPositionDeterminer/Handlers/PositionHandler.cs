using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    internal abstract class PositionHandler
    {
        protected PositionHandler nextHandler;
        NationalResTabRepository _nationalResTabRepository;
        public PositionHandler()
        {
            _nationalResTabRepository = new NationalResTabRepository();
        }

        public void SetNextHandler(PositionHandler handler)
        {
            nextHandler = handler;
        }

        public bool ArePositionsSet(Dictionary<int, List<NationalResultTable>> teamsByPosition)
        {
            return teamsByPosition[0].Count == 0;
        }
        public void DeleteFromDictionary(Dictionary<int, List<NationalResultTable>> teams, List<NationalResultTable> teamsResults)
        {
            if (teams.ContainsKey(0))
            {
                for (int i = teams[0].Count - 1; i >= 0; i--)
                {
                    if (teamsResults.Contains(teams[0][i]))
                    {
                        teams[0].RemoveAt(i);
                    }
                }
            }
        }
        public List<int> SamePositions(Dictionary<int, List<NationalResultTable>> teamsByPosition)
        {
            var keysList = new List<int>();
            foreach (var team in teamsByPosition)
            {
                if (team.Value.Count > 1)
                {
                    keysList.Add(team.Key);
                }
            }
            return keysList;
        }
        public abstract void Handle(Dictionary<int, List<NationalResultTable>> teamsByPosition, List<int> teamsWithSamePositionsKeys = null);
        protected void saveData(Dictionary<int, List<NationalResultTable>> teams, string season)
        {
            for (int i = 1; i < teams.Count; i++)
            {
                teams[i].ForEach(x => x.TotalPosition = i);
                Console.WriteLine($"{teams[i].First().TotalPoints} points - {teams[i].First().TotalPosition} position");
            }
            var allteams = new List<NationalResultTable>();
            foreach (var position in teams)
            {
                allteams.AddRange(position.Value);
            }
            _nationalResTabRepository.Update(allteams, season);
        }
    }
}
