using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace DatabaseLayer.Model
{
    public class ActiveSponsorContract
    {
        public string ID { get; set; }
        public string TeamID { get; set; }
        public string SeasonFrom { get; set; }
        public string SeasonTo { get; set; }
        public double Value { get; set; }
        public int SponsorID { get; set; }
        public ActiveSponsorContract()
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}
