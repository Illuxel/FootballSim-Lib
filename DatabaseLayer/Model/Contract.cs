using System;

namespace DatabaseLayer
{
    public class Contract
    {
        public string Id { get; internal set; }

        public string TeamId { get; set; }
        public Team Team { get; set; }

        public string PersonId { get; set; }
        public Person Person { get; set; }

        public DateTime DateTo { get; set; }
        public DateTime DateFrom { get; set; }

        public double Salary { get; set; }
    }
}