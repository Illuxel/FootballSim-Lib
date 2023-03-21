using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessLogicLayer.Services
{
    internal class FutDbResponseResult
    {
        [JsonPropertyName("pagination")]
        public Dictionary<string, object> Pagination { get; set; }

        [JsonPropertyName("items")]
        public List<Dictionary<string, object>> Items { get; set; }
    }
}
