using DatabaseLayer;
using System;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    internal class MatchEventProcess : IMatchGameEvent
    {
        public string EventCode { get; set; }
        public string EventDescription { get; set; }

        public int Duration { get; set; }
        public int MatchMinute { get; set; }

        public EventLocation Location { get; set; }

        public Dictionary<string, double> NextEventsChances { get; set; }
        public List<IMatchGameEvent>? AdditonalEvents { get; set; }

        public Team HomeTeam { get; set; }
        public Team GuestTeam { get; set; }

        public bool IsBallIntercepted { get; set; }

        public Guid? InjuredPlayer { get; set; }
        public Guid? YellowCardPlayer { get; set; }
        public Guid? RedCardPlayer { get; set; }
        public Guid? ScoredPlayer { get; set; }
        public Guid? AssistedPlayer { get; set; }

        protected MatchEventProcess()
        {
            
        }

        public virtual void ProcessEvent()
        {
            // processing additional events
            if (AdditonalEvents != null)
            {
                for (int i = 0; i < AdditonalEvents.Count; i++)
                {
                    var process = AdditonalEvents[i] as MatchEventProcess;
                    process.HomeTeam = HomeTeam;
                    process.GuestTeam = GuestTeam;
                    process.ProcessEvent();
                }
            }

            // processing base chances
            var processedEventChances = new Dictionary<string, double>();

            foreach (var eventChance in NextEventsChances)
            {
                processedEventChances[eventChance.Key] = MatchEventFactory.GetNextEventValueChance(eventChance.Key, eventChance.Value, HomeTeam, GuestTeam);
            }

            NextEventsChances = processedEventChances;
        }
    }
}
