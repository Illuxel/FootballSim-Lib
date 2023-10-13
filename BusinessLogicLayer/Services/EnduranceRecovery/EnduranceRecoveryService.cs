using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    internal class EnduranceRecoveryService
    {
        private readonly double _enduranceDefaultRecoveryPercent = 0.05;
        private MatchRepository _matchRepository;
        private PlayerRepository _playerRepository;

        public EnduranceRecoveryService()
        {
            _matchRepository = new MatchRepository();
            _playerRepository = new PlayerRepository();
        }

        public void RecoverEndurance(string teamId, DateTime gameDate)
        {
            var recoveryDays = defineRecoveryDays(teamId, gameDate);
            var players = _playerRepository.Retrieve(teamId);

            var recoveredPlayers = getRecoveredPlayers(players, gameDate, recoveryDays);

            _playerRepository.Update(recoveredPlayers);
        }

        public void RecoverEndurance(List<string> teamIds, DateTime gameDate)
        {
            var recoveredPlayers = new List<Player>();

            foreach (var teamId in teamIds)
            {
                var recoveryDays = defineRecoveryDays(teamId, gameDate);
                var players = _playerRepository.Retrieve(teamId);

                var recoveredPlayersFromTeam = getRecoveredPlayers(players, gameDate, recoveryDays);
                
                recoveredPlayers.AddRange(recoveredPlayersFromTeam);
            }

            _playerRepository.Update(recoveredPlayers);
        }

        private List<Player> getRecoveredPlayers(List<Player> players, DateTime gameDate, int recoveryDays)
        {
            var recoveredPlayers = new List<Player>();
            foreach (var player in players)
            {
                if (player.Endurance != 100)
                {
                    var recoveredEnduranceAmount = getRecoveredEnduranceAmount(player, gameDate, recoveryDays);
                    player.Endurance += recoveredEnduranceAmount;
                    if (player.Endurance > 100)
                    {
                        player.Endurance = 100;
                    }
                    recoveredPlayers.Add(player);
                }
            }

            return recoveredPlayers;
        }

        private int getRecoveredEnduranceAmount(Player player, DateTime gameDate, int recoveryDays)
        {
            var age = player.Person.GetAge(gameDate);
            var recoveryCoef = defineRecoveryCoef(age);

            return (int)(player.Endurance * recoveryCoef * recoveryDays * _enduranceDefaultRecoveryPercent);
        }

        private double defineRecoveryCoef(int age)
        {
            if (age <= 24)
            {
                return 1;
            }
            else if (age <= 30)
            {
                return 0.9;
            }
            else if (age <= 36)
            {
                return 0.7;
            }
            else
            {
                return 0.6;
            }
        }

        private int defineRecoveryDays(string teamId, DateTime gameDate)
        {
            var nextMatchDate = _matchRepository.GetNextMatchDate(teamId);
            if (nextMatchDate == null || nextMatchDate < gameDate)
            {
                return 0;
            }
            else
            {
                return (int)(nextMatchDate - gameDate).TotalDays;
            }
        }
    }
}
