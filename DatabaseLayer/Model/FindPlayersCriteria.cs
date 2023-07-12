using Newtonsoft.Json;

namespace DatabaseLayer.Model
{
    public class FindPlayersCriteria
    {
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public string PositionCode { get; set; }
        public int RatingFrom { get; set; }
        public int RatingTo { get; set; }

        public string ConvertToJSON(FindPlayersCriteria findPlayersCriteria)
        {
            return JsonConvert.SerializeObject(findPlayersCriteria, Formatting.Indented);
        }

        public FindPlayersCriteria Deserialize(string findPlayersCriteria)
        {
            return JsonConvert.DeserializeObject<FindPlayersCriteria>(findPlayersCriteria);
        }
    }
}
