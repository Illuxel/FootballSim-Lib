using FootBalLife.Database;
using FootBalLife.Services.MatchGenerator.Events;
using System.Collections.Generic;

namespace FootBalLife.Services.MatchGenerator
{
    public class MatchFactory
    {
        private readonly Team _homeTeam;
        private readonly Team _guestTeam;

        private readonly List<IMatchEvent> _matchEvents;
        /**
         *  штрафний   і пеналті
         */
        private readonly List<IMatchEvent> _additonalEvents;

        public MatchFactory(Team homeTeam, Team guestTeam)
        {
            _homeTeam = homeTeam;
            _guestTeam = guestTeam;

            _matchEvents = new List<IMatchEvent>();
        }
        public void StartMatchGenerating()
        {
            var currentMinute = 0;
            var strategyEventName = "BallControl";

            while (true) 
            {
                var currentEvent = MatchEventFactory.CreateStrategyEvent(strategyEventName, _homeTeam.Strategy, EventLocation.Center) as MatchEventProcess;

                currentEvent.HomeTeam = _homeTeam;
                currentEvent.GuestTeam = _guestTeam;

                while (currentMinute <= 90)
                {
                    currentMinute += currentEvent.Duration;
                    currentEvent.MatchMinute = currentMinute;

                    currentEvent.OnProcessEvent();

                    //Console.WriteLine(currentEvent.EventCode + ':' + currentEvent.Location.ToString() + ':' + currentEvent.HomeTeam.Name);

                    _matchEvents.Add(currentEvent);

                    var nextEvent = MatchEventFactory.CreateNextEvent(currentEvent);
                    currentEvent = nextEvent as MatchEventProcess;

                    if (currentEvent.EventCode == strategyEventName)
                    {
                        break;
                    }
                }
            }
        }

        public void OnTeamChange() { }
        public void OnEventHappened() { }

        public IEnumerable<IMatchEvent> GetResult()
        {
            return _matchEvents;
        }
    }
}