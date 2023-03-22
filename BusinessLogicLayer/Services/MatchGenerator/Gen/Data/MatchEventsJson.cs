using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using DatabaseLayer;

namespace BusinessLogicLayer.Services
{
    internal static class MatchEventsJson
    {
        private static JsonArray? _cachedEvents;

        public static TValue GetValue<TValue>(string fromEventName, string keyName, TValue defaultValue = default)
        {
            loadEventsFromFile();

            if (_cachedEvents == null)
            {
                return defaultValue;
            }

            var eventObject = _cachedEvents.FirstOrDefault(el => el["EventCode"].AsValue().ToString() == fromEventName);
            var deserializedVal = eventObject[keyName].Deserialize<TValue>();

            if (deserializedVal == null)
            {
                return defaultValue;
            }

            return deserializedVal;
        }

        public static Dictionary<string, double> GetEventChances(string fromEventName, EventLocation location)
        {
            var nextChances = GetValue<JsonArray>(fromEventName, "NextEvents");
            var baseEventChancesObject = nextChances?.FirstOrDefault(o => o.AsObject().ContainsKey(location.ToString()));
            var baseEventChances = toDictionaryChances(baseEventChancesObject?[location.ToString()]?.AsArray());

            return baseEventChances;
        }

        public static Dictionary<string, double> GetEventChances(string eventName, EventLocation location, StrategyType strategyType)
        {
            var jsonEventChances = getJsonEventStrategy(eventName, strategyType)?[location.ToString()]?.AsArray();
            var baseEventChances = toDictionaryChances(jsonEventChances);

            return baseEventChances;
        }

        public static Dictionary<string, double>? GetEventAdditionalChances(string fromEventName, EventLocation location)
        {
            var additionalEvents = GetValue<JsonArray>(fromEventName, "AdditionalEvents");

            if (additionalEvents == null)
            {
                return null;
            }
            
            var additionalChancesObject = additionalEvents?.First(o => {
                var obj = o?.AsObject();

                if (obj == null)
                { 
                    return false;
                }

                return obj.ContainsKey(location.ToString());
            });

            var arrayChances = additionalChancesObject?[location.ToString()]?.AsArray();

            return toDictionaryChances(arrayChances);
        }

        public static void ClearCache()
        {
            _cachedEvents = null;
        }

        private static JsonObject? getJsonEventStrategy(string fromEventName, StrategyType strategyType)
        {
            var baseNextEventsChances = GetValue<JsonArray>(fromEventName, "NextEvents");
            var strategyObject = baseNextEventsChances?
                .First(s => s?["Strategy"]?.ToString() == strategyType.ToString())?.AsObject();

            return strategyObject;
        }

        private static Dictionary<string, double> toDictionaryChances(JsonArray? jsonEventChances)
        {
            if (jsonEventChances == null)
            {
                return new Dictionary<string, double>();
            }

            var eventChances = jsonEventChances.ToDictionary(
                k =>
                {
                    string key = k.AsObject().First().Key;
                    return key;
                },
                v =>
                {
                    double value = (double)v.AsObject().FirstOrDefault().Value;
                    return value;
                });

            return eventChances;
        }

        private static void loadEventsFromFile()
        {
            if (_cachedEvents != null)
            {
                return;
            }

            string? jsonFile = null;

            try
            {
                jsonFile = File.ReadAllText(MatchGenSettings.EventsFilePath);
            }
            catch (Exception e)
            {
                _cachedEvents = null;
            }

            if (jsonFile == null)
            {
                return;
            }

            var eventsDoc = JsonDocument.Parse(jsonFile);
            _cachedEvents = eventsDoc.Deserialize<JsonObject>()["Events"].AsArray();
        }
    }
}