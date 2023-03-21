﻿using DatabaseLayer;
using System;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services.MatchGenerator
{
    public interface IMatchGameEvent
    {
        string EventCode { get; set; }
        string EventDescription { get; set; }

        int Duration { get; set; }
        int MatchMinute { get; set; }

        EventLocation Location { get; set; }

        Dictionary<string, double> NextEventsChances { get; set; }
        // TODO: Change to dictionary
        List<IMatchGameEvent>? AdditonalEvents{ get; set; }

        bool IsBallIntercepted { get; set; }

        Team HomeTeam { get; set; }
        Team GuestTeam { get; set; }

        Guid? InjuredPlayer { get; set; }
        Guid? YellowCardPlayer { get; set; }
        Guid? RedCardPlayer { get; set; }
        Guid? ScoredPlayer { get; set; }
        Guid? AssistedPlayer { get; set; }
    }
}