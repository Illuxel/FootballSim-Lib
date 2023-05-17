using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace DatabaseLayer.Model
{
    public class ActiveSponsorContract
    {
        public string ID = Guid.NewGuid().ToString();
        public string TeamID { get; set; }
        public string SeasonFrom { get; set; }
        public string SeasonTo { get; set; }
        public double Value { get; set; }
        public string SponsorID { get; set; }
    }
}
