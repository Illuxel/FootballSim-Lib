using DatabaseLayer;
using DatabaseLayer.Enums;
using DatabaseLayer.Model;
using DatabaseLayer.Repositories;
using System;


namespace BusinessLogicLayer.Services
{
    public class ScoutLookingForPlayers
    {
        PlayerRepository _playerRepository;
        ManagerRequestOfPlayersRepository _managerRequestOfPlayersRepository;
        TransferMarketManager _transferMarketManager;
        public ScoutLookingForPlayers()
        {
            _playerRepository = new PlayerRepository();
            _managerRequestOfPlayersRepository = new ManagerRequestOfPlayersRepository();
            _transferMarketManager = new TransferMarketManager();   
        }

        public bool IsCorrectPlayerForRequest(ManagerRequestOfPlayers request,string playerTeamId,string playerId)
        {
            var player = _playerRepository.RetrieveOne(playerId);
            var playerAge = Convert.ToInt32((request.CreatedDate - player.Person.Birthday).TotalDays / 365.24);
            if (request != null && request.Criteria != null)
            {
                var criteria = request.Criteria;
                if(conditionFilter(criteria.AgeFrom,criteria.AgeTo,playerAge) == false)
                {
                    return false;
                }
                else if (conditionFilter(criteria.RatingFrom, criteria.RatingTo, player.Rating) == false)
                {
                    return false;
                }
                else if (criteria.PositionCode != player.PositionCode)
                {
                    return false;
                }
                else if (_transferMarketManager.MakeOffer(playerId,request.TeamId,playerTeamId,request.BudgetLimit) == false)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public void ConfirmRequest(ManagerRequestOfPlayers request, string playerTeamId,string playerId,DateTime date)
        {
            if(date > request.FinishDate)
            {
                request.Status = ManagerRequestStatus.Failed;
                return;
            }
            
            if(IsCorrectPlayerForRequest(request,playerTeamId,playerId) == false)
            {
                return;
            }
            request.PlayerId = playerId;
            request.Status = ManagerRequestStatus.OnReviewByTheDirector;
            _managerRequestOfPlayersRepository.Update(request);
        }

        private bool conditionFilter(int? conditionMin,int conditionMax,int playerValue)
        {
            if (conditionMin != null)
            {
                if(playerValue < conditionMin)
                {
                    return false;
                }
                else if(playerValue > conditionMax)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
