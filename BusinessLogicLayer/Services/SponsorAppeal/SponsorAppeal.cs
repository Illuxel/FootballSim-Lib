using System;
using DatabaseLayer;
using DatabaseLayer.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    internal class SponsorAppeal
    {
        Sponsor _sponsor;

        TeamRepository _teamRepository;
        SponsorRepository _sponsorRepository;

        List<Sponsor> _sponsors;
        List<Team> _teams;

        Dictionary<Team,int> _groupedTeams;

        public SponsorAppeal() 
        {
            _sponsor = new Sponsor();
            _sponsorRepository = new SponsorRepository();
            _sponsors = _sponsorRepository.Retrieve();
            _teams = _teamRepository.Retrieve().OrderBy(x=>x.CurrentInterlRatingPosition).ToList();
            _groupedTeams = GroupTeamsByRating();
        }

        private Dictionary<Team,int> GroupTeamsByRating()
        {
            _groupedTeams = new Dictionary<Team, int>();

            decimal maxPosition = _teams.Max(x => x.CurrentInterlRatingPosition);
            decimal amount = Math.Floor(maxPosition / 5);

            int category = 1;
            int counter = 1;

            foreach(var team in _teams)
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
                _groupedTeams.Add(team, category);
            }
            return _groupedTeams;
        }
    }
}
