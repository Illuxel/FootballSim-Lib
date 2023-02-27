using FootBalLife.Database;
using System;
using System.Collections.Generic;

namespace FootBalLife.Services.MatchGenerator.Events
{
    internal class MatchEventProcess : IMatchEvent
    {
        public string EventCode { get; set; }
        public string EventDescription { get; set; }

        public int Duration { get; set; }
        public int MatchMinute { get; set; }

        public EventLocation Location { get; set; }

        public Dictionary<string, double> BaseEventsChances { get; set; }
        public Dictionary<string, double> NextEventsChances { get; set; }

        public Team HomeTeam { get; set; }
        public Team GuestTeam { get; set; }

        public Guid? InjuredPlayer { get; }
        public Guid? YellowCardPlayer { get;  }
        public Guid? RedCardPlayer { get; }
        public Guid? ScoredPlayer { get; }
        public Guid? AssistedPlayer { get; set; }

        protected MatchEventProcess() 
        {
            
        }
        /**
         *  Calculates base values and modifiers
         */
        public virtual void OnProcessEvent() 
        {
            NextEventsChances = new Dictionary<string, double>();

            foreach (var eventChance in BaseEventsChances)
            {
                NextEventsChances[eventChance.Key] = MatchEventFactory.GetNextEventValueChance(eventChance.Key, eventChance.Value, HomeTeam, GuestTeam);
            }
        }
    }
}
