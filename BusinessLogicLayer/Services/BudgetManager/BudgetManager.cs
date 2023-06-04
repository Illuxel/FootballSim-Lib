using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BusinessLogicLayer.Services
{
    public class BudgetManager
    {
        ContractRepository _contractRepository;
        TeamRepository _teamRepository;
        public BudgetManager()
        {
            _contractRepository = new ContractRepository();
            _teamRepository = new TeamRepository();
        }

        private List<Contract> getAllContracts(DateTime gameDate)
        {
            var response = _contractRepository.Retrieve(gameDate);
            return response;
        }

        private List<Team> getAllTeams(List<Contract> contracts)
        {
            var teamsId = new HashSet<string>();

            foreach(var contract in contracts)
            {
                teamsId.Add(contract.TeamId);
            }

            return _teamRepository.Retrieve(teamsId.ToList());
        }

        private Dictionary<Team, List<Contract>> getTeamsWithTheirContracts(List<Team> teams, List<Contract> contracts)
        {
            var dict = new Dictionary<Team, List<Contract>>();

            foreach (var team in teams)
            {
                var teamContracts = contracts.Where(x => x.TeamId == team.Id).ToList(); 
                dict.Add(team, teamContracts);
            }
            return dict;
        }
        private List<Team> paySalary(Dictionary<Team, List<Contract>> teamsContractsDict)
        {
            var teams = new List<Team>();
            foreach (var team in teamsContractsDict)
            {
                double amount = team.Value.Sum(x => x.Salary) / 12;
                if (team.Key.Budget != null)
                {
                    team.Key.Budget -= amount / 1000000;
                }
                teams.Add(team.Key);
            }
            return teams;
        }   

        public void PaySalary(DateTime gameDate)
        {
            var contracts = getAllContracts(gameDate);
            var teams = getAllTeams(contracts);

            if(contracts.Count == 0)
            {
                return;
            }

            var teamsContractsDict = getTeamsWithTheirContracts(teams, contracts);

            var payedTeams = paySalary(teamsContractsDict);

            _teamRepository.Update(payedTeams);
        }
    }
}
