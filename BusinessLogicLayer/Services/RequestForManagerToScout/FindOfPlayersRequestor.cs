using DatabaseLayer.Enums;
using DatabaseLayer.Model;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class FindOfPlayersRequestor
    {
        ManagerRequestOfPlayersRepository _managerRequestRep;
        public FindOfPlayersRequestor()
        {
            _managerRequestRep = new ManagerRequestOfPlayersRepository();
        }
        public void Create(string managerId,string teamId, DateTime createDate)
        {
            var request = new ManagerRequestOfPlayers();
            request.ManagerId = managerId;
            request.TeamId = teamId;
            request.CreatedDate = createDate;
            request.Status = ManagerRequestStatus.InProgress;
            _managerRequestRep.Insert(request);
        }

        public List<ManagerRequestOfPlayers> Get(string teamId, ManagerRequestStatus? status = null)
        {
            var response = _managerRequestRep.Retrieve(teamId);
            if (status != null)
            {
                return response.Where(x => x.Status == status.Value).ToList();
            }
            return response;
        }
    }
}
