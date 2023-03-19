using System;
using System.Text.Json.Serialization;

namespace Services.Services
{
    internal class ExternalPlayer
    {
        [JsonPropertyName("id")]
        public int ExtIdPlayer { get; set; }

        [JsonPropertyName("birthDate")]
        public string BirthDate { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("commonName")]
        public string LastName { get; set; }

        [JsonPropertyName("league")]
        public int ExtLeagueId { get; set; }

        [JsonPropertyName("nation")]
        public int CountryId { get; set;}

        [JsonPropertyName("club")]
        public int ExtClubId { get; set; }

        [JsonPropertyName("ratingAverage")]
        public int Rating { get; set; }

        [JsonPropertyName("position")]
        public string Position { get; set; }

        [JsonPropertyName("pace")]
        public int Pace { get; set; }

        [JsonPropertyName("shooting")]
        public int Shooting { get; set; }

        [JsonPropertyName("defending")]
        public int Defending { get; set; }

        [JsonPropertyName("physicality")]
        public int Physicality { get; set; }

        [JsonPropertyName("dribbling")]
        public int Dribbling { get; set; }

        [JsonPropertyName("passing")]
        public int Passing { get; set; }

        //Для воротаря
        [JsonPropertyName("goalkeeperAttributes.reflexes")]
        public int Reflexes { get; set; }

        [JsonPropertyName("goalkeeperAttributes.positioning")]
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
            var value = LastName.Replace(Name, "");
            return value;

        }
    }
}
