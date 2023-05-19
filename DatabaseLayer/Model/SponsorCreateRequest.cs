using DatabaseLayer.Enums;
using System;


namespace DatabaseLayer.Model
{
    public class SponsorCreateRequest
    {
        public string ID { get; internal set; }
        public string TeamID { get; set; }
        public string SeasonFrom { get; set; }
        public string SeasonTo { get; set; }
        public double Value { get; set; }
        public int SponsorID { get; set; }
        public SponsorRequestStatus State { get; set; }
        public SponsorCreateRequest()
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}
