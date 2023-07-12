using DatabaseLayer.Enums;
using DatabaseLayer.Model;
using DatabaseLayer.Repositories;
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

        public void Create(ManagerRequestOfPlayers request)
        {
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
