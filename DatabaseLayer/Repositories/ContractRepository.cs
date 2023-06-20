using Dapper;
using DatabaseLayer.DBSettings;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DatabaseLayer.Repositories
{
    public class ContractRepository
    {
        public List<Contract> Retrieve()
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var response = connection.Query<Contract>("SELECT * FROM Contract").AsList();
                return response;
            }
        }

        public List<Contract> RetrieveByTeam(string teamId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var query = @"SELECT Contract.*, Team.*, Person.*
                    FROM Contract
                    LEFT JOIN Team ON Contract.TeamID = Team.ID
                    LEFT JOIN Person on Contract.PersonID = Person.ID
                    WHERE Contract.TeamID = @teamId";
                var contracts = connection.Query<Contract, Team, Person, Contract>(query,
                    (contract, team, person) =>
                    {
                        contract.Person = person;
                        contract.PersonId = person.Id;
                        contract.Team = team;
                        contract.TeamId = team.Id;
                        return contract;
                    },
                    param: new { teamId },
                    splitOn: "TeamID, PersonID");

                return contracts.ToList();
            }
        }

        public List<Contract> Retrieve(string personId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var query = @"SELECT Contract.*, Team.*, Person.*
                    FROM Contract
                    LEFT JOIN Team ON Contract.TeamID = Team.ID
                    LEFT JOIN Person on Contract.PersonID = Person.ID
                    WHERE Contract.PersonID = @personId";
                var leagues = connection.Query<Contract, Team, Person, Contract>(query,
                    (contract, team, person) =>
                    {
                        contract.Person = person;
                        contract.PersonId = person.Id;
                        contract.Team = team;
                        contract.TeamId = team.Id;
                        return contract;
                    },
                    param: new { personId },
                    splitOn: "TeamID, PersonID"); ;

                return leagues.ToList();
            }
        }

        public Contract RetrieveOne(string contractId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                var query = @"SELECT Contract.*, Team.*, Person.*
                    FROM Contract
                    LEFT JOIN Team ON Contract.TeamID = Team.ID
                    LEFT JOIN Person on Contract.PersonID = Person.ID
                    WHERE Contract.Id = @contractId";
                var contracts = connection.Query<Contract, Team, Person, Contract>(query,
                    (contract, team, person) =>
                    {
                        contract.Person = person;
                        contract.PersonId = person.Id;
                        contract.Team = team;
                        contract.TeamId = team.Id;
                        return contract;
                    },
                    param: new { contractId },
                    splitOn: "TeamID, PersonID"); ;

                return contracts.FirstOrDefault();
            }
        }

        public bool Insert(Contract contract)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                contract.Id = Guid.NewGuid().ToString();
                var record = connection.QuerySingleOrDefault<Agent>("SELECT * FROM contract WHERE ID = @id", new { id = contract.Id });
                bool result = false;
                if (record == null)
                {
                    var rowsAffected = connection.Execute(
                        @"INSERT INTO contract (ID, DateFrom, DateTo, TeamId, PersonId, Salary) 
                            VALUES (@Id, @DateFrom, @DateTo, @TeamId, @PersonId, @Salary)",
                        contract);
                    result = rowsAffected == 1;
                }
                return result;
            }
        }

        public bool Delete(string contractId)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute("DELETE FROM Contract WHERE ID = @contractId ",
                    new { contractId });
                return rowsAffected == 1;
            }
        }
        public List<Contract> Retrieve(DateTime gameDate)
        {
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();
                string formattedDate = gameDate.ToString("yyyy-MM-dd");
                var response = connection.Query<Contract>(
                    "SELECT * FROM Contract WHERE Date(@gameDate) BETWEEN DateFrom AND DateTo",
                    new { gameDate = formattedDate }).AsList();
                return response;
            }
        }
    }
}
