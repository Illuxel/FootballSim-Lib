using DatabaseLayer;
using DatabaseLayer.Repositories;
using System.Collections.Generic;

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
            var allteams = new List<NationalResultTable>();
            foreach (var league in teams)
            {
                allteams.AddRange(league.Value);
            }
            _nationalResTabRepository.Update(allteams, season);
        }
    }
}
