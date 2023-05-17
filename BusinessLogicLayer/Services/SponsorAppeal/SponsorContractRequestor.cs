using System;
using DatabaseLayer;
using DatabaseLayer.Repositories;
using System.Collections.Generic;
using System.Linq;
using DatabaseLayer.Model;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Mapping;

namespace BusinessLogicLayer.Services
{
    public class SponsorContractRequestor
    {
        string _teamID;
        int _gameYear;

        TeamRepository _teamRepository;
        ActiveSponsorContractRepository _activeSponsorContractRepository;
        SponsorRepository _sponsorRepository;
        SeasonValueCreator _seasonValueCreator;

        List<Team> _teams;
        List<Sponsor> _sponsors;
        List<ActiveSponsorContract> _activeSponsorContrats;

        Dictionary<string, int> _groupedTeams;

        public SponsorContractRequestor(string teamId,int gameYear)
        {
            _teamID = teamId;
            _gameYear = gameYear;

            _seasonValueCreator = new SeasonValueCreator();

            _teamRepository = new TeamRepository();
            _activeSponsorContractRepository = new ActiveSponsorContractRepository();

            _teams = new List<Team>();
            _sponsors = new List<Sponsor>();
            _activeSponsorContrats = new List<ActiveSponsorContract>();

            _groupedTeams = new Dictionary<string, int>();
        }
        private Dictionary<string, int> groupTeamsByRating()
        {
            _groupedTeams = new Dictionary<string, int>();

            decimal maxPosition = _teams.Max(x => x.CurrentInterlRatingPosition);
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
            _teams = _teamRepository.Retrieve();
            _groupedTeams = groupTeamsByRating();

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


        public void AppealContract()
        {
            _activeSponsorContrats = _activeSponsorContractRepository.Retrieve(_teamID);
            if (_activeSponsorContrats.Count > 3)
            {
                return;
            }
            _activeSponsorContractRepository.Insert(GetRandomContract());
        }

        ActiveSponsorContract GetRandomContract()
        {
            var random = new Random();
            var contract = new ActiveSponsorContract
            {
                Value = getContractAmount(_teamID),
                SponsorID = FindRandomUniqueSponsor(),
                SeasonFrom = _seasonValueCreator.GetSeason(_gameYear),
                SeasonTo = _seasonValueCreator.GetSeason(_gameYear + random.Next(1, 3)),
                TeamID = _teamID
            };
            return contract;

        }
        int FindRandomUniqueSponsor()
        {
            _sponsors = _sponsorRepository.Retrieve();

            var random = new Random();
            int randomIndex = random.Next(0, _sponsors.Count() - 1);
            var sponsor = _sponsors[randomIndex].ID;
            while (CheckUniqueContract(sponsor) == false)
            {
                sponsor = _sponsors[random.Next(0, _sponsors.Count() - 1)].ID;
            }

            return sponsor;

        }
        bool CheckUniqueContract(int sponsorId)
        {
            if (_activeSponsorContrats == null)
            {
                return false;
            }
            else
            {
                return _activeSponsorContrats.Where(x => x.TeamID == _teamID && x.SponsorID == sponsorId).Count() == 0;
            }
        }
    }
}
