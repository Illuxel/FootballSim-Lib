using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLayer
{
    public class PlayerStatistic
    {
        public Player Player { get; set; }
        public string PlayerId { get; set; }
        public int CountOfGoals { get; set; }
        public int CountOfAssists { get; set; }
        public int CountOfPlayedMatches { get; set; }
        public string Season { get; set; }
    }
}
