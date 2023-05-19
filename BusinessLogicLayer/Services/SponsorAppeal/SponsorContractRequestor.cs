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
        TeamRepository _teamRepository;
        SponsorCreateRequestRepository _sponsorCreateRequestRepository;
        SeasonValueCreator _seasonValueCreator;


        public SponsorContractRequestor()
        {
            _teamRepository = new TeamRepository();
            _sponsorCreateRequestRepository = new SponsorCreateRequestRepository(); 
            _seasonValueCreator = new SeasonValueCreator();
        }

        public List<SponsorCreateRequest> CreateContractRequests(string teamId, int gameYear)
        {
            Random random = new Random();

            int maxContractsCount = 3;
            int maxContractsRequests = 3;

            var teams = _teamRepository.Retrieve();

            var groupedTeams = getGroupedTeamsByRating(teams);

            var contractsRequestsCount = random.Next(0, maxContractsRequests);

            var activeSponsorContrats = _sponsorCreateRequestRepository.Retrieve(teamId);

            var sponsors = _sponsorCreateRequestRepository.RetrieveFreeSponsor(teamId);

            var contractRequests = new List<SponsorCreateRequest>();

            if (activeSponsorContrats.Count < maxContractsCount && contractsRequestsCount != 0)
            {
                for (int i = 0; i < contractsRequestsCount; i++)
                {
                    var contract = getRandomContract(teamId, gameYear, groupedTeams, sponsors);
                    if(_sponsorCreateRequestRepository.Insert(contract))
                    {
                        contractRequests.Add(contract);
                    }
                }
            }
            return contractRequests;
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
        private Dictionary<string, int> getGroupedTeamsByRating(List<Team> teams)
        {
            var groupedTeams = new Dictionary<string, int>();


            decimal maxPosition = teams.Count();
            decimal amount = Math.Floor(maxPosition / 5);

            int category = 1;
            int counter = 1;

            foreach (var team in teams)
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
                groupedTeams.Add(team.Id, category);
            }
            return groupedTeams;
        }

        private double getContractAmount(Dictionary<string, int> groupedTeams, string teamId)
        {
            Random random = new Random();
            if (groupedTeams.TryGetValue(teamId, out var value))
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

        private SponsorCreateRequest getRandomContract(string teamId,int gameYear,Dictionary<string,int> groupedTeams,List<Sponsor> sponsors)
        {
            Random random = new Random();
            return new SponsorCreateRequest
            {
                Value = getContractAmount(groupedTeams,teamId),
                SponsorID = findRandomUniqueSponsor(teamId,sponsors),
                SeasonFrom = _seasonValueCreator.GetSeason(gameYear),
                SeasonTo = _seasonValueCreator.GetSeason(gameYear + random.Next(1, 3)),
                TeamID = teamId,
                State = SponsorRequestStatus.Waiting,
            };
        }

        private int findRandomUniqueSponsor(string teamId,List<Sponsor> sponsors)
        {
            Random random = new Random();
            int randomIndex = random.Next(0, sponsors.Count() - 1);

            var sponsor = sponsors[randomIndex];

            return sponsor.ID;
        }
    }
}
