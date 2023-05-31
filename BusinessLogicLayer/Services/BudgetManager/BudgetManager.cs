using DatabaseLayer;
using DatabaseLayer.Repositories;
using System.Collections.Generic;
using System.Linq;

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

        private List<Team> getAllTeams()
        {
            return _teamRepository.Retrieve();
        }

        private List<Contract> getAllContracts()
        {
            return _contractRepository.Retrieve();
        }

        private Dictionary<Team, List<Contract>> getTeamsWithTheirContracts(List<Team> teams, List<Contract> contracts)
        {
            var dict = new Dictionary<Team, List<Contract>>();

            foreach (var team in teams)
            {
                var teamContracts = new List<Contract>();

                foreach (var contract in contracts)
                {
                    if (contract.TeamId == team.Id)
                    {
                        teamContracts.Add(contract);
                    }
                }

                dict.Add(team, teamContracts);
            }
            return dict;
        }

        private Dictionary<Team, double> countAmount(Dictionary<Team, List<Contract>> teamsContractsDict)
        {
            var dict = new Dictionary<Team, double>();
            foreach (var team in teamsContractsDict)
            {
                double amount = team.Value.Sum(x => x.Salary) / 12;
                dict.Add(team.Key, amount);
            }
            return dict;
        }

        private List<Team> determineRest(Dictionary<Team, double> teamsWithAmounts)
        {
            foreach (var team in teamsWithAmounts)
            {
                if (team.Key.Budget != null)
                {
                    team.Key.Budget -= team.Value / 1000000;
                }
                /*else
                {
                    Чи потрібно якось опрацьовувати нульові бюджети команд?
                    Також бюджет може перейти в мінус
                }*/
            }
            return teamsWithAmounts.Keys.ToList();
        }

        public void PaySalary()
        {
            var teams = getAllTeams();
            var contracts = getAllContracts();

            var teamsContractsDict = getTeamsWithTheirContracts(teams, contracts);

            var teamsAmount = countAmount(teamsContractsDict);

            var payedTeams = determineRest(teamsAmount);

            _teamRepository.Update(payedTeams);
        }
    }
}