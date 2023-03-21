﻿using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services.MatchGenerator
{
    internal static class MatchEventFactory
    {
        // creates event without base chances
        public static IMatchGameEvent? CreateEvent(string eventName)
        {
            var nameParts = eventName.Split('.');

            var eventCode = nameParts[0];
            var eventArg = nameParts.Length > 1 ? nameParts[1] : "";

            IMatchGameEvent? matchEvent = createEmptyEvent(eventCode);

            if (matchEvent == null)
            {
                return matchEvent;
            }

            matchEvent.EventCode =          eventCode;
            matchEvent.EventDescription =   MatchEventsJson.GetValue<string>(eventCode, "EventDescription");
            matchEvent.Duration =           MatchEventsJson.GetValue<int>(eventCode, "Duration");

            matchEvent.IsBallIntercepted =  MatchEventsJson.GetValue<bool>(eventCode, "IsBallIntercepted");

            if (!(matchEvent is BallControlEvent) || matchEvent is BallControlEvent && !eventArg.Any())
            {
                var locationName = MatchEventsJson.GetValue<string>(eventCode, "BaseEventLocation");
                matchEvent.Location = Enum.Parse<EventLocation>(locationName);
            }
            else
            {
                var location = Enum.Parse<EventLocation>(eventArg);
                matchEvent.Location = location;
            } 
            
            return matchEvent;
        }

        public static IMatchGameEvent? CreateStrategyEvent(string eventName, StrategyType strategyType, EventLocation location)
        {
            IMatchGameEvent? matchEvent = CreateEvent(eventName);

            if (matchEvent == null)
            {
                return matchEvent;
            }

            matchEvent.NextEventsChances = MatchEventsJson.GetEventChances(eventName, location, strategyType);  

            return matchEvent;
        }

        public static IMatchGameEvent? CreateNextEvent(IMatchGameEvent fromCurrent)
        {
            var nextEventName = ResultFromChances.Next(fromCurrent.NextEventsChances);
            var nextEvent = CreateEvent(nextEventName);

            if (nextEvent == null)
            {
                return nextEvent;
            }

            nextEvent.HomeTeam = fromCurrent.HomeTeam;
            nextEvent.GuestTeam = fromCurrent.GuestTeam;

            if (nextEvent.Location == EventLocation.None)
            {
                nextEvent.Location = fromCurrent.Location;
            }

            nextEvent.NextEventsChances = MatchEventsJson.GetEventChances(nextEvent.EventCode, nextEvent.Location);

            var additionalChances = MatchEventsJson.GetEventAdditionalChances(fromCurrent.EventCode, fromCurrent.Location);

            if (additionalChances != null)
            {
                var nextAdditionalEventName = ResultFromChances.Next(additionalChances);
                var nextAdditionalEvent = CreateEvent(nextAdditionalEventName);

                nextEvent.AdditonalEvents = new List<IMatchGameEvent>() { nextAdditionalEvent };

                // TODO: Create more than 1 event in additional
                //       Add to json file None chance to additional events
            }

            return nextEvent;
        }

        public static double GetNextEventValueChance(string eventName, double baseValue, Team homeTeam, Team guestTeam)
        {
            double nextChance = baseValue;

            switch (eventName)
            {
                case "BallStrike":
                    //avg(техніка)/avg(техніка_суперника) * avg(удар)/avg(фізика_суперника)
                    nextChance = baseValue * (homeTeam.AvgTechnique() / guestTeam.AvgTechnique())
                        * (homeTeam.AvgStrike(PlayerPosition.Attack) / homeTeam.AvgPhysicalTraining());
                    break; 
                case "BallStrikeMissed":
                    // (60/техніка_гравця)*(60/удар_гравця)
                    nextChance = baseValue * (60 / homeTeam.GetPlayer(PlayerPosition.Attack).Dribbling)
                        * (60 / homeTeam.GetPlayer(PlayerPosition.Attack).Strike);
                    break;
                case "BallStrikeGoal":
                    // (техніка_гравця/позиція_голкіпера)*(удар_гравця/реакція_голкіпера)
                    nextChance = baseValue * (homeTeam.GetPlayer(PlayerPosition.Attack).Dribbling / guestTeam.GetPlayer(PlayerPosition.Goalkeeper).Physics)
                        * (homeTeam.GetPlayer(PlayerPosition.Attack).Strike / homeTeam.GetPlayer(PlayerPosition.Goalkeeper).Endurance);
                    break;
                case "BallStrikeSave":
                    // (позиція_голкіпера/техніка_гравця)*(реакція_голкіпера/удар_гравця)
                    nextChance = baseValue * (guestTeam.GetPlayer(PlayerPosition.Goalkeeper).Physics / homeTeam.GetPlayer(PlayerPosition.Attack).Dribbling)
                        * (guestTeam.GetPlayer(PlayerPosition.Goalkeeper).Endurance / homeTeam.GetPlayer(PlayerPosition.Attack).Strike);
                    break;
                case "BallControl.HomePart":
                case "BallControl.Center":
                case "BallControl.GuestPart":
                    // avg(передача)/avg(захист_суперника)
                    nextChance = baseValue * homeTeam.AvgPassing() / guestTeam.AvgDefense(PlayerPosition.Defence);
                    break;
                case "BallInterception":
                    // avg(захист_суперника)/avg(передача)
                    nextChance = baseValue * guestTeam.AvgDefense(PlayerPosition.Defence) / homeTeam.AvgPassing();
                    break;
                case "FreeKick":
                    // avg(техніка)/avg(техніка_суперника)
                    nextChance = baseValue * homeTeam.AvgTechnique() / guestTeam.AvgTechnique();
                    break;
                case "Penalty":
                    // avg(техніка)/avg(техніка_суперника) * avg(швидкість)/avg(швидкість_суперника)
                    nextChance = baseValue * (homeTeam.AvgTechnique() / guestTeam.AvgTechnique())
                        * (homeTeam.AvgSpeed() / guestTeam.AvgSpeed());
                    break;
                case "BallOut":
                    // avg(захист_суперника)/avg(передача) * avg(фізика_суперника)/avg(фізика)
                    nextChance = baseValue * (guestTeam.AvgDefense(PlayerPosition.Defence) / homeTeam.AvgPassing())
                        * (guestTeam.AvgPhysicalTraining() / homeTeam.AvgPhysicalTraining());
                    break;
            }

            return nextChance;
        }

        private static IMatchGameEvent? createEmptyEvent(string fromName)
        {
            IMatchGameEvent? matchEvent;

            switch (fromName)
            {
                case "BallStrike":
                    matchEvent = new BallStrikeEvent();
                    break;
                case "BallStrikeMissed":
                    matchEvent = new BallStrikeMissedEvent();
                    break;
                case "BallStrikeGoal":
                    matchEvent = new BallStrikeGoalEvent();
                    break;
                case "BallStrikeSafe":
                    matchEvent = new BallStrikeSafeEvent();
                    break;
                case "BallControl":
                    matchEvent = new BallControlEvent();
                    break;
                case "BallInterception":
                    matchEvent = new BallInterceptionEvent();
                    break;
                case "BallOut":
                    matchEvent = new BallOutEvent();
                    break;
                case "Penalty":
                    matchEvent = new PenaltyEvent();
                    break;
                case "FreeKick":
                    matchEvent = new FreeKickEvent();
                    break;
                case "YellowCard":
                    matchEvent = new YellowCardEvent();
                    break;
                case "RedCard":
                    matchEvent = new RedCardEvent();
                    break;
                case "PlayerInjury":
                    matchEvent = new PlayerInjuryEvent();
                    break;
                default:
                    throw new Exception($"Unknown name for event: {fromName}");
            }

            return matchEvent;
        }
    }
}
