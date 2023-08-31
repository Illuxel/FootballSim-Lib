using System;

namespace DatabaseLayer
{
    public class JobRequest
    {
        public string ID { get; internal set; }
        public string PersonID { get; set; }
        public string TeamID { get; set; }
        public double Salary { get; set; }
        public DateTime DurationTo { get; set; }
    }
}