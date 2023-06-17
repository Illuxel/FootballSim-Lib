using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{ 
    internal class RedCardEvent : MatchEventProcess
    {

        private EventLocationConverter _eventLocationConverter;
        public RedCardEvent()
        {
            _eventLocationConverter = new EventLocationConverter();
        }
        public override void ProcessEvent()
        {
            if (this.Location != EventLocation.None)
            {
                var convertorResponse = _eventLocationConverter.Convert(this.Location);
                var fieldPartPosition = convertorResponse == DatabaseLayer.PlayerFieldPartPosition.Attack ? PlayerFieldPartPosition.Defence : 
                    convertorResponse == DatabaseLayer.PlayerFieldPartPosition.Defence ? PlayerFieldPartPosition.Attack :
                    PlayerFieldPartPosition.Midfield;



                var playerId = this.GuestTeam.GetPlayer(fieldPartPosition).PersonID;
                this.RedCardPlayer = new Guid(playerId);
                /*foreach(var player in GuestTeam.MainPlayers)
                {
                    if(player.Value.CurrentPlayer.PersonID == playerId)
                    {
                        player.Value.CurrentPlayer = null;
                        GuestTeam.AvailablePlayerCount -= 1;
                        break;
                    }
                }*/
            }
        }
    }
}