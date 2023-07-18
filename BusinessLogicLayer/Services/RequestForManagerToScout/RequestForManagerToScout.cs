using System;

namespace BusinessLogicLayer.Services
{
    public class RequestForManagerToScout
    {
        FindOfPlayersRequestor _findOfPlayersRequestor;
        RequestForScout _requestForScout;
        string _requestId;
        public RequestForManagerToScout()
        {
            _findOfPlayersRequestor = new FindOfPlayersRequestor();
            _requestForScout = new RequestForScout(); 
        }

        public void FindPlayers(string managerId, string teamId, DateTime createDate)
        {
            _findOfPlayersRequestor.Create(managerId, teamId, createDate);
            _requestId = _findOfPlayersRequestor.GetLastRequestId();
        }
        public void RequestForScout(DateTime finishDate, int ageTo, int ratingTo, string positionCode, int bugetLimit, int? ageFrom = null, int? ratingFrom = null)
        {
            _requestForScout.NewRequest(_requestId, finishDate, ageTo, ratingTo, positionCode, bugetLimit, ageFrom, ratingFrom);
        }

        public void DeclineRequest()
        {
            _requestForScout.DeclineRequest(_requestId);
        }
    }
}
