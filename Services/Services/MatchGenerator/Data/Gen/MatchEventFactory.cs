using FootBalLife.Database;
using FootBalLife.Database.Models;

using FootBalLife.Services.MatchGenerator.Events;

namespace FootBalLife.Services.MatchGenerator
{
    internal static class MatchEventFactory
    {
        //Getting match event from string
        public static IMatchEvent? CreateEmptyEvent(string fromName)
        {
            IMatchEvent? matchEvent;

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
        public static IMatchEvent? CreateStrategyEvent(string fromEventName, StrategyType strategyType, EventLocation location)
        {
            IMatchEvent? matchEvent = MatchEvents.GetEvent(fromEventName);

            if (matchEvent == null)
            {
                return matchEvent;
            }

            matchEvent.BaseEventsChances = MatchEvents.GetEventStrategyChances(fromEventName, strategyType, location);  

            return matchEvent;
        }

        public static IMatchEvent CreateNextEvent(IMatchEvent fromCurrent)
        {
            var nextEventName = ChoiseResult.Next(fromCurrent.NextEventsChances);
            var nextMatchEvent = MatchEvents.GetEvent(nextEventName);

            var homeTeam = fromCurrent.HomeTeam;
            var guestTeam = fromCurrent.GuestTeam;

            nextMatchEvent.HomeTeam = homeTeam;
            nextMatchEvent.GuestTeam = guestTeam;

            if (nextMatchEvent.Location == EventLocation.None)
            {
                nextMatchEvent.Location = fromCurrent.Location;
            }

            nextMatchEvent.BaseEventsChances = MatchEvents.GetBaseEventChances(nextMatchEvent.EventCode, nextMatchEvent.Location);

            return nextMatchEvent;
        }

        public static double GetNextEventValueChance(string eventName, double baseValue, Team homeTeam, Team guestTeam)
        {
            double finalValue = 0.0f;

            switch (eventName)
            {
                case "BallStrike":
                    //avg(техніка)/avg(техніка_суперника) * avg(удар)/avg(фізика_суперника)
                    finalValue = baseValue * (homeTeam.AvgTechnique() / guestTeam.AvgTechnique())
                        * (homeTeam.AvgStrike(PlayerPostion.Attacker) / homeTeam.AvgPhysicalTraining());
                    break;
                case "BallStrikeMissed":
                    // (60/техніка_гравця)*(60/удар_гравця)
                    finalValue = baseValue * (60 / homeTeam.GetPlayer(PlayerPostion.Attacker).Technique) 
                        * (60 / homeTeam.GetPlayer(PlayerPostion.Attacker).Strike);
                    break;
                case "BallStrikeGoal":
                    // (техніка_гравця/позиція_голкіпера)*(удар_гравця/реакція_голкіпера)
                    finalValue = baseValue * (homeTeam.GetPlayer(PlayerPostion.Attacker).Technique / guestTeam.GetPlayer(PlayerPostion.GoalKeeper).Physics) 
                        * (homeTeam.GetPlayer(PlayerPostion.Attacker).Strike / homeTeam.GetPlayer(PlayerPostion.GoalKeeper).Endurance);
                    break;
                case "BallStrikeSave":
                    // (позиція_голкіпера/техніка_гравця)*(реакція_голкіпера/удар_гравця)
                    finalValue = baseValue * (guestTeam.GetPlayer(PlayerPostion.GoalKeeper).Physics / homeTeam.GetPlayer(PlayerPostion.Attacker).Technique) 
                        * (guestTeam.GetPlayer(PlayerPostion.GoalKeeper).Endurance / homeTeam.GetPlayer(PlayerPostion.Attacker).Strike);
                    break;
                case "BallControl.HomePart": 
                case "BallControl.Center":
                case "BallControl.GuestPart":
                    // avg(передача)/avg(захист_суперника)
                    finalValue = baseValue * homeTeam.AvgPassing() / guestTeam.AvgDefense(PlayerPostion.Defender);
                    break;
                case "BallInterception":
                    // avg(захист_суперника)/avg(передача)
                    finalValue = baseValue * guestTeam.AvgDefense(PlayerPostion.Defender) / homeTeam.AvgPassing();
                    break;
                case "FreeKick":
                    // avg(техніка)/avg(техніка_суперника)
                    finalValue = baseValue * homeTeam.AvgTechnique() / guestTeam.AvgTechnique();
                    break;
                case "Penalty":
                    // avg(техніка)/avg(техніка_суперника) * avg(швидкість)/avg(швидкість_суперника)
                    finalValue = baseValue * (homeTeam.AvgTechnique() / guestTeam.AvgTechnique()) 
                        * (homeTeam.AvgSpeed() / guestTeam.AvgSpeed());
                    break;
                case "BallOut.FromAllie":
                    // avg(захист_суперника)/avg(передача) * avg(фізика_суперника)/avg(фізика)
                    finalValue = baseValue * (guestTeam.AvgDefense(PlayerPostion.Defender) / homeTeam.AvgPassing())
                        * (guestTeam.AvgPhysicalTraining() / homeTeam.AvgPhysicalTraining());
                    break;
                case "BallOut.FromEnemy":
                    // avg(захист_суперника)/avg(передача)
                    finalValue = baseValue * (guestTeam.AvgDefense(PlayerPostion.Defender) / homeTeam.AvgPassing());
                    break;
            }

            return finalValue;
        }
    }
}
