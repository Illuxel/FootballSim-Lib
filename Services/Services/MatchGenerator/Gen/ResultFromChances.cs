using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services.MatchGenerator
{
    public class ResultFromChances
    {
        private readonly IDictionary<string, double> _eventChance;

        public ResultFromChances(IDictionary<string, double> eventChance)
        {
            _eventChance = eventChance;
        }

        public string Next()
        {
            return nextChoiseChance();
        }
        public static string Next(IDictionary<string, double> eventChances)
        {
            if (!eventChances.Any())
            {
                return string.Empty;
            }

            if (eventChances.Count == 1)
            {
                return eventChances.Keys.FirstOrDefault();
            }

            var rand = new Random();

            var randNum = rand.NextDouble();
            var totalChance = randNum * eventChances.Values.Sum();

            foreach (var eventChance in eventChances)
            {
                if (eventChance.Value >= totalChance)
                {
                    return eventChance.Key;
                }
                totalChance -= eventChance.Value;
            }

            return string.Empty;
        }
        private string nextChoiseChance()
        {
            return Next(_eventChance);
        }
    }
}
