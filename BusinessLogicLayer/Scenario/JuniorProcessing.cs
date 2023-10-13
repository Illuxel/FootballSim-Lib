using BusinessLogicLayer.Services;
using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;

namespace BusinessLogicLayer.Scenario
{
    internal class JuniorProcessing
    {
        private static int _countOfCountries = 10;
        private readonly int _juniorMaxYear = 21;

        JuniorPersonGeneration _juniorPersonGeneration;
        SeasonValueCreator _seasonValueCreator;
        PlayerRepository _playerRepository;
        public JuniorProcessing()
        {
            _juniorPersonGeneration = new JuniorPersonGeneration();
            _seasonValueCreator = new SeasonValueCreator();
            _playerRepository = new PlayerRepository();
        }

        public void GenerateNewJuniorPerson(string teamId, DateTime gameDate, int? countryId = null, string? name = null, string? surname = null)
        {
            if(isGenerating(gameDate))
            {
                countryId = countryId == null ? randomCountryId() : (countryId > _countOfCountries ? randomCountryId() : countryId);

                var birthDate = randomBday(gameDate);

                _juniorPersonGeneration.GenerateNewJuniorPerson(teamId, gameDate.Year, birthDate, (int)countryId, name, surname);
            }
        }

        public void DeleteIsJuniorStatus(string playerId, DateTime gameDate)
        {
            if(!string.IsNullOrEmpty(playerId))
            {
                var player = _playerRepository.RetrieveOne(playerId);
                deleteIsJuniorStatus(player, gameDate);
                _playerRepository.Update(player);
            }
        }

        public void DeleteIsJuniorStatus(Player player, DateTime gameDate)
        {
            deleteIsJuniorStatus(player, gameDate);
            _playerRepository.Update(player);
        }

        public void DeleteIsJuniorStatus(DateTime gameDate)
        {
            var players = _playerRepository.RetrieveAllJuniors();
            var playersForUpdate = new List<Player>();

            foreach(var player in players)
            {
                deleteIsJuniorStatus(player, gameDate);
                playersForUpdate.Add(player);
            }

            _playerRepository.Update(playersForUpdate);
        }

        private void deleteIsJuniorStatus(Player player,DateTime gameDate)
        {
            if (player != null && player.IsJunior == 1)
            {
                var age = player.Person.GetAge(gameDate);
                if (age >= _juniorMaxYear)
                {
                    player.IsJunior = 0;
                }
            }
        }

        private DateTime randomBday(DateTime gameDate)
        {
            DateTime minDate = gameDate.AddYears(-18);
            DateTime maxDate = gameDate.AddYears(-21);

            if (maxDate < minDate)
            {
                // Swap minDate and maxDate if maxDate is smaller
                DateTime tempDate = minDate;
                minDate = maxDate;
                maxDate = tempDate;
            }

            var random = new Random();
            int totalDays = (int)(maxDate - minDate).TotalDays;
            int randomDays = random.Next(0, totalDays);

            return minDate.AddDays(randomDays);
        }


        private int randomCountryId()
        {
            var random = new Random();

            return random.Next(1, _countOfCountries);
        }

        private bool isGenerating(DateTime gameDate)
        {
            var startDate = _seasonValueCreator.GetSeasonStartDate(gameDate.Year);
            if(gameDate < startDate)
            {
                startDate = _seasonValueCreator.GetSeasonStartDate(gameDate.Year - 1);
            }

            var genDate = startDate.AddMonths(3);
            
            for(int i = 1; i <= 3; i++)
            {
                if(gameDate == genDate)
                {
                    return true;
                }
                genDate = genDate.AddMonths(3);
            }
            return false;
        }
    }
}
