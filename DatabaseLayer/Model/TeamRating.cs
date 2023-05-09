using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLayer.Model
{
    internal class TeamRating
    {
        public string TeamId { get; set; }
        public int CurrentPosition { get; set; }
        public double? PercentPerSeason1 { get; set; }
        public double? PercentPerSeason2 { get; set; }
        public double? PercentPerSeason3 { get; set; }
        public double? PercentPerSeason4 { get; set; }
        public double? PercentPerSeason5 { get; set; }
    }
}
