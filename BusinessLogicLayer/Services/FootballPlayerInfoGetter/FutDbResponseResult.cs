using Newtonsoft.Json;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    internal class FutDbResponseResult
    {
        [JsonProperty("pagination")]
        public Dictionary<string, object> Pagination { get; set; }

        [JsonProperty("items")]
        public List<Dictionary<string, object>> Items { get; set; }
    }
}
