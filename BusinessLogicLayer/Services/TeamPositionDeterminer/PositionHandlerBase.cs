using DatabaseLayer;
using DatabaseLayer.Repositories;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    internal abstract class PositionHandlerBase
    {
        protected PositionHandlerBase nextHandler;
        private NationalResTabRepository _nationalResTabRepository;
        protected string _season;
        public PositionHandlerBase(string season)
        {
            _nationalResTabRepository = new NationalResTabRepository();
            _season = season;
        }

        public void SetNextHandler(PositionHandlerBase handler)
        {
            nextHandler = handler;
        }

        public bool ArePositionsSet(Dictionary<int, List<NationalResultTable>> teams)
        {
            return teams[0].Count == 0;
        }
        public void DeleteFromDictionary(Dictionary<int, List<NationalResultTable>> teams, List<NationalResultTable> teamsResults)
        {
            if (teams.TryGetValue(0, out var results))
            {
                results.RemoveAll(item => teamsResults.Contains(item));
            }
        }
        public List<int> SamePositions(Dictionary<int, List<NationalResultTable>> teams)
        {
            var keysList = new List<int>();
            foreach (var team in teams)
            {
                if (team.Value.Count > 1)
                {
                    keysList.Add(team.Key);
                }
            }
            return keysList;
        }
        public abstract void Handle(Dictionary<int, List<NationalResultTable>> teams, List<int> teamsWithSamePositionsKeys = null);
        protected void saveData(Dictionary<int, List<NationalResultTable>> teams, string season)
        {
            for (int i = 1; i < teams.Count; i++)
            {
                teams[i].ForEach(x => x.TotalPosition = i);
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
