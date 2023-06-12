using DatabaseLayer;

namespace BusinessLogicLayer.Services
{
    public enum EventLocation
    {
        None,
        HomePart,
        Center,
        GuestPart
    }

    public class EventLocationConverter
    {
        public EventLocation Convert(PlayerFieldPartPosition playerFieldPartPosition)
        {
            switch (playerFieldPartPosition)
            {
                case PlayerFieldPartPosition.Defence: return EventLocation.HomePart;
                case PlayerFieldPartPosition.Midfield: return EventLocation.Center;
                case PlayerFieldPartPosition.Attack: return EventLocation.GuestPart;
                default: return EventLocation.None;
            }
        }

        public PlayerFieldPartPosition Convert(EventLocation eventLocation)
        {
            switch (eventLocation)
            {
                case EventLocation.HomePart: return PlayerFieldPartPosition.Defence;
                case EventLocation.Center: return PlayerFieldPartPosition.Midfield;
                case EventLocation.GuestPart: return PlayerFieldPartPosition.Attack;
                default: return PlayerFieldPartPosition.All;
            }
        }
    }
}