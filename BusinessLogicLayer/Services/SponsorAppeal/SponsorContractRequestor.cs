using System;
using DatabaseLayer;
using DatabaseLayer.Enums;
using DatabaseLayer.Repositories;
using System.Collections.Generic;
using System.Linq;
using DatabaseLayer.Model;

namespace BusinessLogicLayer.Services
{
    public class SponsorContractRequestor
    {
        Random _random;

        TeamRepository _teamRepository;
        SponsorCreateRequestRepository _sponsorCreateRequestRepository;
        SponsorRepository _sponsorRepository;

        List<Team> _teams;
        List<Sponsor> _sponsors;

        Dictionary<string, int> _groupedTeams;

        SeasonValueCreator _seasonValueCreator;

        public SponsorContractRequestor()
        {
            _random = new Random();
            var seasonValueCreator = new SeasonValueCreator();
            _teamRepository = new TeamRepository();
            _sponsorCreateRequestRepository = new SponsorCreateRequestRepository();
        }


        private Dictionary<string, int> groupTeamsByRating()
        {
            _groupedTeams = new Dictionary<string, int>();

            decimal maxPosition = _teams.Count();
            decimal amount = Math.Floor(maxPosition / 5);

            int category = 1;
            int counter = 1;

            foreach (var team in _teams)
            {
                if (counter <= amount)
                {
                    counter++;
                }
                else
                {
                    category++;
                    counter = 1;

                }
                _groupedTeams.Add(team.Id, category);
            }
            return _groupedTeams;
        }

        private double getContractAmount(string teamId)
        {
            Random random = new Random();

            if (_teams == null)
            {
                _teams = _teamRepository.Retrieve();
            }
            if (_groupedTeams == null)
            {
                _groupedTeams = groupTeamsByRating();

            }
            if (_groupedTeams.TryGetValue(teamId, out var value))
            {
                return value switch
                {
                    1 => random.Next(14, 20),
                    2 => random.Next(10, 14),
                    3 => random.Next(7, 10),
                    4 => random.Next(4, 7),
                    5 => random.Next(2, 4),
                    _ => 0
                } * 1000000;
            }
            else
            {
                return 0;
            }
        }

        public List<SponsorCreateRequest> CreateContractRequests(string teamId, int gameYear)
        {
            int maxContractsCount = 3;
            int maxContractsRequests = 3;

            var contractsRequestsCount = _random.Next(0, maxContractsRequests);

            var activeSponsorContrats = _sponsorCreateRequestRepository.Retrieve(teamId);

            if (activeSponsorContrats.Count >= maxContractsCount || contractsRequestsCount == 0)
            {
                return new List<SponsorCreateRequest>();
            }
            else
            {
                var contractRequests = new List<SponsorCreateRequest>();
                for(int i = 0;i < contractsRequestsCount;i++)
                {
                    var contract = getRandomContract(teamId, gameYear);
                    contractRequests.Add(contract);
                    _sponsorCreateRequestRepository.Insert(contract);
                }
                return contractRequests;
            }
        }

        public void DeleteExpiredContracts(int gameYear)
        {
            var expiredYear = gameYear - 1;
            var season = _seasonValueCreator.GetSeason(expiredYear);
            _sponsorCreateRequestRepository.DeleteExpired(season);
        }

        public void ClaimContract(string teamId, SponsorCreateRequest contract)
        {
            contract.State = SponsorRequestStatus.Active;
            _sponsorCreateRequestRepository.UpdateState(contract);
            _sponsorCreateRequestRepository.DeleteCanceled(teamId);
        }

        private SponsorCreateRequest getRandomContract(string teamId,int gameYear)
        {
            var contract = new SponsorCreateRequest
            {
                Value = getContractAmount(teamId),
                SponsorID = findRandomUniqueSponsor(teamId),
                SeasonFrom = _seasonValueCreator.GetSeason(gameYear),
                SeasonTo = _seasonValueCreator.GetSeason(gameYear + _random.Next(1, 3)),
                TeamID = teamId,
                State = SponsorRequestStatus.Waiting,
            };
            return contract;

        }

        private int findRandomUniqueSponsor(string teamId)
        {
            if(_sponsors == null)
            {
                _sponsors = _sponsorRepository.Retrieve();
            }
            int randomIndex = _random.Next(0, _sponsors.Count() - 1);

            var sponsor = _sponsors[randomIndex];
            while (_sponsorCreateRequestRepository.IsUnique(teamId, sponsor.ID) == false)
            {
                sponsor = _sponsors[_random.Next(0, _sponsors.Count() - 1)];
            }

            return sponsor.ID;
        }
    }
}
