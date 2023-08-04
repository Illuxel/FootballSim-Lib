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

        public string SeasonTo { get; set; }
        public string SeasonFrom { get; set; }

        public DateTime? DateFrom { get; set; }
        public string DateFromString 
        {
            get
            {
                if(DateFrom == null)
                {
                    return DateTime.MinValue.ToString("yyyy-MM-dd");
                }
                else
                {
                    return DateFrom.Value.ToString("yyyy-MM-dd");
                }
            }
        }
        public DateTime? DateTo { get; set; }
        public string DateToString { 
            get 
            {
                if(DateTo == null)
                {
                    return DateTime.MinValue.ToString("yyyy-MM-dd");
                }
                else
                {
                    return DateTo.Value.ToString("yyyy-MM-dd");
                }
            } 
        }

        public double Salary { get; set; }
    }

}