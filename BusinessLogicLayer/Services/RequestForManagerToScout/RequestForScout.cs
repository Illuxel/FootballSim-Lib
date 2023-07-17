using DatabaseLayer.Repositories;
using System;
using System.CodeDom;

namespace BusinessLogicLayer.Services
{
    public class RequestForScout
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
            requestFromManager.Criteria.AgeTo = ageTo;
            requestFromManager.Criteria.RatingTo = ratingTo;
            requestFromManager.Criteria.PositionCode = positionCode;
            if(ageFrom != null)
            {
                requestFromManager.Criteria.AgeFrom = ageFrom;
            }
            if(ratingFrom != null)
            {
                requestFromManager.Criteria.RatingFrom = ratingFrom;
            }
            _managerRequestOfPlayersRepository.Update(requestFromManager);
        }
    }  
}
