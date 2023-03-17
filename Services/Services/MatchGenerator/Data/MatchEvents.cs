using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

using FootBalLife.Database;
using FootBalLife.Services.MatchGenerator.Events;

namespace FootBalLife.Services.MatchGenerator
{
    internal static class MatchEvents
    {
        private static JsonDocument? _parsedEvents;

        // якщо location = EventLocation.None, то не присваює базові шанси
        public static IMatchEvent? GetEvent(string fromEventName)
        {
            var splitedName = fromEventName.Split('.');
            var eventNamePart = splitedName[0];
            var eventArgPart = "";

            if (splitedName.Count() > 1)
            {
                eventArgPart = splitedName[1];
            }

            IMatchEvent? matchEvent = MatchEventFactory.CreateEmptyEvent(eventNamePart);

            if (matchEvent == null)
            {
                return matchEvent;
            }

            matchEvent.EventCode = eventNamePart;
            matchEvent.EventDescription = getJsonValue<string>(eventNamePart, "EventDescription");

            matchEvent.Duration = getJsonValue<int>(eventNamePart, "Duration");

            if (eventNamePart != "BallControl")
            {
                var locationName = getJsonValue<string>(eventNamePart, "BaseEventLocation");
                matchEvent.Location = Enum.Parse<EventLocation>(locationName);
            }
            else
            {
                if (eventArgPart.Any())
                {
                    matchEvent.Location = Enum.Parse<EventLocation>(eventArgPart);
                }
                else
                {
                    var locationName = getJsonValue<string>(eventNamePart, "BaseEventLocation");
                    matchEvent.Location = Enum.Parse<EventLocation>(locationName);
                }
            }

            return matchEvent;
        }
        public static Dictionary<string, double> GetBaseEventChances(string fromEventName, EventLocation location = EventLocation.None)
        {
            var currentEventObject = getJsonEvent(fromEventName);
            var nextLocationsChances = currentEventObject["BaseNextEvents"].AsArray();

            var fieldName = location.ToString();

            var baseEventChancesObject = nextLocationsChances.FirstOrDefault(o => o.AsObject().ContainsKey(fieldName));
            var baseEventChances = toDictionaryChances(baseEventChancesObject[fieldName].AsArray());

            return baseEventChances;
        }
        public static Dictionary<string, double> GetEventStrategyChances(string fromEventName, StrategyType strategyType, EventLocation location)
        {
            var strategyObject = getJsonEventStrategy(fromEventName, strategyType);
            var jsonEventChances = strategyObject[location.ToString()]
                .AsArray();

            var baseEventChances = toDictionaryChances(jsonEventChances);

            return baseEventChances;
        }

        public static void ClearCache()
        {
            _parsedEvents = null;
        }

        private static JsonObject getJsonEvent(string fromName)
        {
            loadEventsFromFile();

            var eventsObject = _parsedEvents
                .Deserialize<JsonObject>();

            var eventsArray = eventsObject["Events"]
                .AsArray();

            var currentEventObject = eventsArray.FirstOrDefault(e => e["EventCode"].ToString() == fromName)
                .AsObject();

            return currentEventObject;
        }
        private static JsonObject getJsonEventStrategy(string fromEventName, StrategyType strategyType)
        {
            var currentEventObject = getJsonEvent(fromEventName);

            var baseNextEventsChances = currentEventObject["BaseNextEvents"]
                .AsArray();

            var currentStrategyObject = baseNextEventsChances.First(
                s => s["Strategy"].ToString() == strategyType.ToString())
                .AsObject();

            return currentStrategyObject;
        }

        private static T getJsonValue<T>(string fromName, string fieldName) 
        {
            var jsonEvent = getJsonEvent(fromName);
            var jsonValue = jsonEvent[fieldName].AsValue();

            return jsonValue.Deserialize<T>();
        }

        private static Dictionary<string, double> toDictionaryChances(JsonArray jsonEventChances)
        {
            var eventChances = jsonEventChances.ToDictionary(
                k => {
                    string key = k.AsObject().First().Key;
                    return key;
                },
                v => {
                    double value = (double)v.AsObject().First().Value;
                    return value;
                });

            return eventChances;
        }
        private static void loadEventsFromFile() 
        {
            if (_parsedEvents != null) 
            {
                return;
            }

            try
            {
                string jsonFile = File.ReadAllText("EventsData.json");
                _parsedEvents = JsonDocument.Parse(jsonFile);
            }
            catch (Exception e) 
            {
                // writing to log

                _parsedEvents = null;
            }
        }
    }
}