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
                var query = @"SELECT Contract.*, Team.*, Person.*
                    FROM Contract
                    LEFT JOIN Team ON Contract.TeamID = Team.ID
                    LEFT JOIN Person on Contract.PersonID = Person.ID";
                var leagues = connection.Query<Contract, Team, Person, Contract>(query,
                    (contract, team, person) =>
                    {
                        contract.Person = person;
                        contract.Team = team;
                        return contract;
                    },
                    splitOn: "TeamID, PersonID");

                return leagues.ToList();
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
                        contract.Team = team;
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
                    WHERE c.PersonID = @personId";
                var leagues = connection.Query<Contract, Team, Person, Contract>(query,
                    (contract, team, person) =>
                    {
                        contract.Person = person;
                        contract.Team = team;
                        return contract;
                    },
                    param: new { personId },
                    splitOn: "TeamID, PersonID"); ;

                return leagues.ToList();
            }
        }

        public Contract RetrieveOne(string contractId)
        {
            var query = @"SELECT Contract.*, Team.*, Person.*
                    FROM Contract
                    LEFT JOIN Team ON Contract.TeamID = Team.ID
                    LEFT JOIN Person on Contract.PersonID = Person.ID
                    WHERE l.Id = @contractId";
            using (var connection = new SQLiteConnection(DatabaseManager.ConnectionString))
            {
                connection.Open();

                using (var multi = connection.QueryMultiple(query, new { contractId }))
                {
                    var contract = multi.Read<Contract>().FirstOrDefault();
                    var team = multi.Read<Team>().FirstOrDefault();
                    var person = multi.Read<Person>().FirstOrDefault();
                    contract.Team = team;
                    contract.Person = person;
                    return contract;
                }
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
                        @"INSERT INTO contract (ID, SeasonFrom, SeasonTo, TeamId, PersonId, Price) 
                            VALUES (@Id, @SeasonFrom, @SeasonTo, @TeamId, @PersonId, @Price)",
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
    }
}
