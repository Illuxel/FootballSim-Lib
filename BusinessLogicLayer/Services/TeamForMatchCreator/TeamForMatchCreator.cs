using DatabaseLayer;
using DatabaseLayer.Repositories;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class TeamForMatchCreator
    {
        private TeamRepository _teamRepository;
        private PlayerPositionDeterminer _playerPositionDeterminer;
        private static int _defaultSparePlayersCount = 7;
        public TeamForMatchCreator() 
        {
            _teamRepository = new TeamRepository();
            _playerPositionDeterminer = new PlayerPositionDeterminer();
        }
        public ITeamForMatch Create(Team team, int sparePlayersCount = 0)
        {
            if(sparePlayersCount == 0)
            {
                sparePlayersCount = _defaultSparePlayersCount;
            }

            var teamForMatch = new TeamForMatch();
            teamForMatch.Id = team.Id;
            teamForMatch.Name = team.Name;
            teamForMatch.BaseColor = team.BaseColor;
            teamForMatch.Strategy = team.Strategy;
            teamForMatch.TacticSchema = team.TacticSchema;
            teamForMatch.MainPlayers = _playerPositionDeterminer.GetPlayersWithPosition(team.TacticSchema, team.Players);
            teamForMatch.SparePlayers = team.Players.Except(teamForMatch.MainPlayers.Select(pl => pl.Value.CurrentPlayer)).OrderByDescending(pl => pl.CurrentPlayerRating).Take(sparePlayersCount).ToList();
            return teamForMatch;
        }

        public ITeamForMatch Create(string teamId, int sparePlayersCount = 0)
        {
            return Create(_teamRepository.Retrive(teamId), sparePlayersCount);
        }
    }
}
