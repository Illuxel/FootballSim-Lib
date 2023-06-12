using System;

namespace BusinessLogicLayer.Services
{
    internal class PlayerInjuryEvent : MatchEventProcess
    {
        // при травмі гравця, діалог вікно

        private EventLocationConverter _eventLocationConverter;
        public PlayerInjuryEvent()
        {
            _eventLocationConverter = new EventLocationConverter();
        }
        public override void ProcessEvent()
        {
            if (this.Location != EventLocation.None)
            {
                var fieldPartPosition = _eventLocationConverter.Convert(this.Location);
                this.InjuredPlayer = new Guid(this.GuestTeam.GetPlayer(fieldPartPosition).PersonID);
            }
        }
    }
}
