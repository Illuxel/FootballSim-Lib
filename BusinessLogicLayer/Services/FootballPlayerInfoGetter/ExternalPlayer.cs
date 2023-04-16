using Newtonsoft.Json;
using System;

namespace BusinessLogicLayer.Services
{
    internal class ExternalPlayer
    {
        [JsonProperty("id")]
        public int ExtIdPlayer { get; set; }

        [JsonProperty("birthDate")]
        public string BirthDate { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("commonName")]
        public string LastName { get; set; }

        [JsonProperty("league")]
        public int ExtLeagueId { get; set; }

        [JsonProperty("nation")]
        public int CountryId { get; set;}

        [JsonProperty("club")]
        public int ExtClubId { get; set; }

        [JsonProperty("ratingAverage")]
        public int Rating { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("pace")]
        public int Pace { get; set; }

        [JsonProperty("shooting")]
        public int Shooting { get; set; }

        [JsonProperty("defending")]
        public int Defending { get; set; }

        [JsonProperty("physicality")]
        public int Physicality { get; set; }

        [JsonProperty("dribbling")]
        public int Dribbling { get; set; }

        [JsonProperty("passing")]
        public int Passing { get; set; }

        //Для воротаря
        [JsonProperty("goalkeeperAttributes.reflexes")]
        public int Reflexes { get; set; }

        [JsonProperty("goalkeeperAttributes.positioning")]
        public int Positioning { get; set; }
        public DateTime GetBirthDate()
        {
            var dateFormat = "YYYY-MM-DD";
            if(!DateTime.TryParse(BirthDate, out DateTime value))
            {
                value = DateTime.MinValue;
            }
            return value;
        }
        public string GetFirstName()
        {
            var value = Name.Replace(LastName, "");
            return value;

        }
    }
}
