using DatabaseLayer.Repositories;
using System;

namespace BusinessLogicLayer.Services
{
    internal class RequestForScout
    {
        ManagerRequestOfPlayersRepository _managerRequestOfPlayersRepository;
        public RequestForScout()
        {
            _managerRequestOfPlayersRepository = new ManagerRequestOfPlayersRepository();
        }

        public void DeclineRequest(string id)
        {
            var request = _managerRequestOfPlayersRepository.RetrieveById(id);
            if (request != null)
            {
                request.Status = DatabaseLayer.Enums.ManagerRequestStatus.Failed;
            }
        }

        public void NewRequest(string id, DateTime finishDate,int ageTo,int ratingTo,string positionCode,int bugetLimit, int? ageFrom = null,int? ratingFrom = null)
        { 
            var requestFromManager = _managerRequestOfPlayersRepository.RetrieveById(id);
            requestFromManager.BudgetLimit = bugetLimit;
            requestFromManager.FinishDate = finishDate;
            var criteria = requestFromManager.Criteria;
            criteria.AgeTo = ageTo;
            criteria.RatingTo = ratingTo;
            criteria.PositionCode = positionCode;
            if(ageFrom != null)
            {
                criteria.AgeFrom = ageFrom;
            }
            if(ratingFrom != null)
            {
                criteria.RatingFrom = ratingFrom;
            }
            requestFromManager.Criteria = criteria;
            _managerRequestOfPlayersRepository.Update(requestFromManager);
        }
    }  
}
