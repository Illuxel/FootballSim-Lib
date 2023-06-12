using System;

namespace BusinessLogicLayer.Services
{
    internal class YellowCardEvent : MatchEventProcess
    {
        private EventLocationConverter _eventLocationConverter;
        public YellowCardEvent()
        {
            _eventLocationConverter = new EventLocationConverter();
        }
        public override void ProcessEvent()
        {
            if(this.Location != EventLocation.None )
            {
                var fieldPartPosition = _eventLocationConverter.Convert(this.Location);
                this.YellowCardPlayer = new Guid(this.GuestTeam.GetPlayer(fieldPartPosition).PersonID);
            }
            // YellowCardPlayer = Player
            // guest team player
            // if 2 yellow give red card
            // insert to nextchances red card
        }
    }
}
