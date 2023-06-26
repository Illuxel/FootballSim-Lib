using DatabaseLayer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    internal class BaseMatchEventModel
    {

        public BaseMatchEventModel()
        {
            _nextEvents = new List<Dictionary<string, object>>();
            _additionalEvents = new List<Dictionary<string, object>>();
        }
        public string EventCode { get; set; }
        public string EventDescription { get; set; }
        public int Duration { get; set; }
        public EventLocation BaseEventLocation { get; set; }
        public bool IsBallIntercepted {get; set; }

        [JsonProperty("NextEvents")]
        internal List<Dictionary<string, object>> _nextEvents { get; set; }

        private Dictionary<string, Dictionary<string, double>> _nextEventsChances;

        [JsonProperty("AdditionalEvents")]
        internal List<Dictionary<string, object>> _additionalEvents { get; set; }
        public Dictionary<string, double> GetNextEvents(EventLocation location, StrategyType strategyType = StrategyType.BallСontrol)
        {
            //if (_nextEventsChances == null)
            {
                _nextEventsChances = new Dictionary<string, Dictionary<string, double>>();
            }
            try
            {
                var hasStrategy = false;
                var chances = new Dictionary<string, object>();
                foreach(var nextEvent in _nextEvents)
                {
                    if (nextEvent.TryGetValue("Strategy", out object strategyName) && strategyName.ToString() == strategyType.ToString())
                    {
                        chances = nextEvent;
                        hasStrategy = true;
                        break;
                    }
                }
                if(!hasStrategy)
                {
                    foreach (var nextEventsByLocation in _nextEvents)
                    {
                        foreach(var nextEvent in nextEventsByLocation)
                        {
                            chances[nextEvent.Key] = nextEvent.Value;
                        }
                    }
                }

                foreach (var chanceByStr in chances.Where(item=>item.Key != "Strategy"))
                {
                    _nextEventsChances[chanceByStr.Key] = getBaseEventChances(chanceByStr.Value);
                }
            }
            catch (Exception error)
            {
                throw error;
            }
            _nextEventsChances.TryGetValue(location.ToString(), out Dictionary<string, double> totalChances);
            return totalChances;

        }
        public Dictionary<string, double> GetAdditionalEvents(EventLocation location)
        {
            if(_additionalEvents.Count == 0)
            {
                return new Dictionary<string, double>();
            }
            var strLocation = location.ToString();
            var chancesFirst = _additionalEvents.FirstOrDefault(item => item.ContainsKey(strLocation))?[strLocation];
            var additionalEventsChances = getBaseEventChances(chancesFirst);
            return additionalEventsChances;

        }
        private static Dictionary<string, double> getBaseEventChances(object jsonObject)
        {
            var dict = new Dictionary<string, double>();
            JArray jArray = JArray.Parse(jsonObject.ToString());
            foreach (JObject jObject in jArray)
            {
                foreach (KeyValuePair<string, JToken> pair in jObject)
                {
                    dict.Add(pair.Key, (double)pair.Value);
                }
            }
            return dict;
        }

        private static Dictionary<string, double> getBaseEventChances(Dictionary<string, object> jsonChances)
        {
            var dict = new Dictionary<string, double>();
            foreach (var pair in jsonChances)
            {
                dict.Add(pair.Key, (double)pair.Value);
            }
            return dict;
        }

    }
}
