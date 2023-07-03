using System.Collections.Generic;
using DatabaseLayer.Repositories;

namespace BusinessLogicLayer.Services
{
    public class PlayerInvolvementLastMatchChecker
    {
        private Dictionary<string, List<string>> _cache;
        private PlayerInMatchRepository _playerInMatchRepository;
        public PlayerInvolvementLastMatchChecker()
        {
            _playerInMatchRepository = new PlayerInMatchRepository();
        }

        public bool Check(string teamId, string playerId)
        {
            if(_cache == null)
            {
                fillCache();
            }
            return _cache.TryGetValue(teamId, out List<string> players) && players.Contains(playerId);
        }

        private void fillCache()
        {
            _cache = _playerInMatchRepository.RetrieveByLastMatches();
        }
    }
}
