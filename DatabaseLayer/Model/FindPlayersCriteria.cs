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

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented);
        }
    }
}
