using System;

namespace BusinessLogicLayer.Services
{
    public class RequestForManagerToScout
    {
        FindOfPlayersRequestor _findOfPlayersRequestor;
        RequestForScout _requestForScout;
        public RequestForManagerToScout()
        {
            _findOfPlayersRequestor = new FindOfPlayersRequestor();
            _requestForScout = new RequestForScout(); 
        }

        public string FindPlayersAndGetRequestId(string managerId, string teamId, DateTime createDate)
        {
            _findOfPlayersRequestor.Create(managerId, teamId, createDate);
            return _findOfPlayersRequestor.GetLastRequestId();
        }
        public void RequestForScout(string requestId,DateTime finishDate, int ageTo, int ratingTo, string positionCode, int bugetLimit, int? ageFrom = null, int? ratingFrom = null)
        {
            _requestForScout.NewRequest(requestId, finishDate, ageTo, ratingTo, positionCode, bugetLimit, ageFrom, ratingFrom);
        }

        public void DeclineRequest(string requestId)
        {
            _requestForScout.DeclineRequest(requestId);
        }
    }
}
