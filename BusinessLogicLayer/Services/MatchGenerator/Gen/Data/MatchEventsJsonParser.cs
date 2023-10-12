using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DatabaseLayer;
using Newtonsoft.Json;

namespace BusinessLogicLayer.Services
{
    internal class MatchEventsJsonParser
    {
        private static Dictionary<string, BaseMatchEventModel> _cachedEvents;
        public static BaseMatchEventModel GetEvent(string eventName)
        {
            if (_cachedEvents == null)
            {
                loadEventsFromFile();
            }

            if (!_cachedEvents.TryGetValue(eventName, out BaseMatchEventModel baseEvent))
            {
                return null;
            }

            return baseEvent;
        }
        public static Dictionary<string, double> GetEventChances(string matchEventCode, EventLocation location)
        {
            var matchEvent = GetEvent(matchEventCode);
            return matchEvent.GetNextEvents(location);
        }

        public static Dictionary<string, double> GetEventChances(string matchEventCode, EventLocation location, StrategyType strategyType)
        {
            var matchEvent = GetEvent(matchEventCode);
            return matchEvent.GetNextEvents(location, strategyType);
        }


        public static Dictionary<string, double>? GetEventAdditionalChances(string fromEventName, EventLocation location)
        {
            var currentEvent = GetEvent(fromEventName);
            return currentEvent.GetAdditionalEvents(location);
        }

        public static void ClearCache()
        {
            _cachedEvents = null;
        }

        private static bool loadEventsFromFile()
        {
            if (_cachedEvents != null)
            {
                return true;
            }
            _cachedEvents = new Dictionary<string, BaseMatchEventModel>();
            try
            {
                string? jsonString = File.ReadAllText(MatchGenSettings.EventsFilePath);
                if (jsonString == null)
                {
                    return false;
                }

                _cachedEvents = JsonConvert.DeserializeObject<List<BaseMatchEventModel>>(jsonString).ToDictionary(
                    key => key.EventCode,
                    value => value);

                return _cachedEvents != null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
