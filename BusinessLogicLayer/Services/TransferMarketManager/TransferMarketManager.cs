using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using DatabaseLayer;
using BusinessLogicLayer.Rules;
using System.Linq;
using DatabaseLayer.Model;

namespace BusinessLogicLayer.Services
{
    public class TransferMarketManager
    {
        #region private fields

        private int _minCountInTeam = 18;
        private decimal _coefficientFroBuy = 1.5m;
        private TeamRepository _teamRepository;
        private TransferMarketRepository _transferMarketRepository;
        private TransferOfferRepository _transferOfferRepository;
        private TransferRepository _transferRepository;

        #endregion
        public TransferMarketManager()
        {
            _teamRepository= new TeamRepository();
            _transferMarketRepository = new TransferMarketRepository();
            _transferOfferRepository = new TransferOfferRepository();
            _transferRepository = new TransferRepository();
        }

        /// <summary>
        /// Submit for transfer
        /// </summary>
        /// <param name="player"></param>
        /// <param name="team"></param>
        /// <param name="typeTransfer"></param>
        /// <param name="sum"></param>
        public bool SubmitForTransfer(string playerId, string teamId, TransferType typeTransfer, decimal sum, string ?newId = null)
        {
            // Checking the number of players in the team, must be more than 18
            var teamInfo = _teamRepository.Retrieve(teamId);
            if (teamInfo == null)
            {
                return false;
            }
            if (teamInfo.Players.Count <= _minCountInTeam)
            {
                return false;
            }

            if (_transferMarketRepository.RetrieveByPlayer(playerId).Any())
            {
                return false;
            }

            var market = new TransferMarket();
            market.Id = string.IsNullOrEmpty(newId) ? Guid.NewGuid().ToString() : newId;
            market.IDPlayer = playerId;
            market.IDTeam = teamId;
            market.Agreement = EnumDescription.GetEnumDescription(typeTransfer);
            market.DesireAmount = sum;

            return _transferMarketRepository.Insert(market);
        }

        /// <summary>
        /// Remove from the transfer
        /// </summary>
        /// <param name="transferMarketId"></param>
        /// <returns></returns>
        public bool RemoveFromTransfer(string transferMarketId)
        {
            return _transferMarketRepository.Delete(transferMarketId);
        }

        /// <summary>
        /// Make an offer for a player not from the market
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="teamId"></param>
        public bool MakeOffer(string playerId, string teamId, string teamIdBuyer, decimal sum)
        {
            var idMarket = Guid.NewGuid().ToString();
            if (!SubmitForTransfer(playerId,teamId, TransferType.Offer, sum * _coefficientFroBuy, idMarket))
            {
                return false;
            }
            return MakeOffer(idMarket, teamIdBuyer, sum * _coefficientFroBuy);
        }

        /// <summary>
        /// Make an offer for a player from the market
        /// </summary>
        /// <param name="idMarket"></param>
        public bool MakeOffer(string idMarket, string teamIdBuyer, decimal sum)
        {
            var offer = new TransferOffer();
            offer.Id = Guid.NewGuid().ToString();
            offer.TeamIdBuyer = teamIdBuyer;
            offer.IDMarket = idMarket;
            offer.OfferSum= sum;

            return _transferOfferRepository.Insert(offer);
        }

        /// <summary>
        /// Search on the transfer market
        /// </summary>
        /// <param name="byRatting">Rating from</param>
        /// <param name="byPosition">Position</param>
        /// <param name="ageLowerBound">Lower age limit</param>
        /// <param name="ageUpperBound">Upper age limit</param>
        /// <param name="sumLowerBound">The lower limit of the amount</param>
        /// <param name="sumUpperBound">The upper limit of the amount</param>
        /// <returns></returns>
        public List<TransferMarket> SearchPlayerOnMarket(TransferMarketSearchParams transfer)
        {
            return _transferMarketRepository.RetrieveByParameters(transfer);
        }

        /// <summary>
        /// Прийняти/відхилити пропозицію
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="status"></param>
        /// <param name="dateRelease"></param>
        /// <param name="sum"></param>
        public bool AcceptCancelOffer(string offer, TransferStatus status, DateTime dateRelease, decimal sum )
        {
            if(EnumDescription.GetEnumDescription(status).Equals("DONE"))
            {
                var transfer = new TransferJournal();
                transfer.Id = Guid.NewGuid().ToString();
                transfer.OfferId = offer;
                transfer.Status = EnumDescription.GetEnumDescription(status);
                transfer.SumFact = sum;
                transfer.DateRelease = dateRelease;

                if (_transferRepository.Insert(transfer))
                {
                    return RemoveFromTransfer(_transferOfferRepository.Retrieve(offer).IDMarket);
                }
                return false;
            }
            return false;         
        }

        /// <summary>
        /// Змінити угоду
        /// </summary>
        /// <param name="offerId"></param>
        /// <param name="sum"></param>
        public void ChangeOffer(string offerId, decimal sum)
        {
            var offer = _transferOfferRepository.Retrieve(offerId);
            offer.OfferSum = sum;
            _transferOfferRepository.Update(offer);
        }

        /// <summary>
        /// Отримати список пропозицій (які отримав)
        /// </summary>
        /// <returns></returns>
        public List<TransferOffer> GetOfferByTeamSeller(string teamIdSeller)
        {
            return _transferOfferRepository.RetrieveByTeamSeller(teamIdSeller);
        }

        /// <summary>
        /// Отримати список пропозицій (які зробив)
        /// </summary>
        /// <returns></returns>
        public List<TransferOffer> GetOfferByTeamBuyer(string teamIdSeller)
        {
            return _transferOfferRepository.RetrieveByTeamBuyer(teamIdSeller);
        }
    }
}
