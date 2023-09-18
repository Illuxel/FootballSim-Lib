using System;
using System.Security.Cryptography.X509Certificates;

namespace DatabaseLayer
{
    public class JobRequest
    {
        public string ID { get; }
        public string PersonID { get; set; }
        public string TeamID { get; set; }
        public double Salary { get; set; }
        public DateTime DurationTo { get; set; }

        public JobRequest() 
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}