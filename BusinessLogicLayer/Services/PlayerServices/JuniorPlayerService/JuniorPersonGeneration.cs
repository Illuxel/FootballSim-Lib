using DatabaseLayer;
using DatabaseLayer.Enums;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    public class JuniorPersonGeneration
    {
        Player _juniorPlayer;
        Person _juniorPerson;
        PersonRepository _personRepository;
        PlayerRepository _playerRepository;
        PersonNameGenaration _personNameGenaration;
        PlayerGeneration _playerGeneration;
        SeasonValueCreator _seasonValueCreator;
        ContractRepository _contractRepository;

        Dictionary<int, List<PlayerPosition>> _positionsByKey;

        public JuniorPersonGeneration()
        {
            _personRepository = new PersonRepository();
            _playerRepository = new PlayerRepository();
            _personNameGenaration = new PersonNameGenaration();
            _playerGeneration = new PlayerGeneration();
            _seasonValueCreator = new SeasonValueCreator();
            _contractRepository = new ContractRepository();
        }
        public void GenerateNewJuniorPerson(string teamId, int gameYear, DateTime birthDate, int countryId, string? name = null, string? surname = null)
        {
            if (name != null || surname != null)
            {
                name = name == null ? string.Empty : name;
                surname = surname == null ? string.Empty : surname;

                _juniorPerson = new Person();
                setPersonNameSurname(name, surname);
            }
            else
            {
                _juniorPerson = _personNameGenaration.CreatePersonName(countryId);
            }

            createNewJuniorPerson(birthDate, countryId);
            createNewJuniorPlayer(teamId, gameYear);
        }

        private void setPositionDict()
        {
            if (_positionsByKey == null)
            {
                _positionsByKey = new Dictionary<int, List<PlayerPosition>>
                {
                    {
                        1, new List< PlayerPosition>()
                        {
                            PlayerPosition.Goalkeeper
                        }
                    },
                    {
                        2, new List < PlayerPosition >()
                        {
                            PlayerPosition.CentralDefender,
                            PlayerPosition.RightDefender ,
                            PlayerPosition.LeftDefender,
                            PlayerPosition.RightFlankDefender,
                            PlayerPosition.LeftFlankDefender
                        }
                    },
                    {   3, new List < PlayerPosition >()
                        {
                            PlayerPosition.CentralDefensiveMidfielder,
                            PlayerPosition.CentralMidfielder,
                            PlayerPosition.RightMidfielder,
                            PlayerPosition.LeftMidfielder,
                            PlayerPosition.CentralAttackingMidfielder
                        }
                    },
                    {   4, new List < PlayerPosition >()
                        {
                            PlayerPosition.Forward,
                            PlayerPosition.CentralForward,
                            PlayerPosition.RightOffensive,
                            PlayerPosition.LeftOffensive
                        }
                    }
                };

            }
        }

        private void createNewJuniorPerson(DateTime birthDate, int countryId)
        {
            setBirthDate(birthDate);
            setCountryId(countryId);

            _personRepository.Insert(_juniorPerson);
        }

        private void createNewJuniorPlayer(string teamID, int gameYear)
        {
            int maxRating = 60, minRating = 45;

            var midRating = (maxRating + minRating) / 2;

            setPositionDict();

            var position = randomPositionProcessing();

            _juniorPlayer = _playerGeneration.GeneratePlayer(position, midRating);
            setPlayerPersonId();
            setJuniorStatus();

            createNewContract(teamID, gameYear);
            _playerRepository.Insert(_juniorPlayer);
        }
        private void setPersonNameSurname(string name, string surname)
        {
            _juniorPerson.Name = name;
            _juniorPerson.Surname = surname;
        }

        private void setBirthDate(DateTime birthDate)
        {
            _juniorPerson.Birthday = birthDate;
        }

        private void setPlayerPersonId()
        {
            _juniorPlayer.PersonID = _juniorPerson.Id;
        }

        private void setJuniorStatus()
        {
            _juniorPlayer.IsJunior = 1;
        }

        private void setCountryId(int countryId)
        {
            _juniorPerson.CountryID = countryId;
        }



        private PlayerPosition randomPositionProcessing()
        {
            int gkChance = 5;
            int defChance = 20;
            int midChance = 30;

            Random random = new Random();
            int randomNum = random.Next(0, 100);

            //1 - GK
            //2 - DEF
            //3 - MID
            //4 - ATT

            if (randomNum < gkChance)
            {
                return positionCode(1);
            }
            else if (randomNum < defChance + gkChance)
            {
                return positionCode(2);
            }
            else if (randomNum < midChance + defChance + gkChance)
            {
                return positionCode(3);
            }
            else
            {
                return positionCode(4);
            }
        }

        private PlayerPosition positionCode(int posKey)
        {
            var positions = _positionsByKey[posKey];

            Random random = new Random();
            int randomNum = random.Next(0, positions.Count);

            return positions[randomNum];
        }

        private void createNewContract(string teamId, int gameYear)
        {
            var contract = new Contract();
            contract.PersonId = _juniorPerson.Id;
            contract.TeamId = teamId;
            var startSeason = _seasonValueCreator.GetSeason(gameYear);
            var endSeason = _seasonValueCreator.GetFutureSeason(startSeason, 3);
            contract.SeasonFrom = startSeason;
            contract.SeasonTo = endSeason;
            contract.Salary = getJuniorContractSalary();

            _contractRepository.Insert(contract);
            _juniorPlayer.ContractID = contract.Id;
        }

        private int getJuniorContractSalary()
        {
            int minSalary = 216;
            int maxSalary = 234;

            Random random = new Random();
            int randomNum = random.Next(minSalary, maxSalary);

            return randomNum * 1000;
        }
    }
}
