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

        public string DateTo { get; set; }
        public string DateFrom { get; set; }

        public double Salary { get; set; }


        public Contract GetNewContract()
        {
            var random = new Random();
            return new Contract()
            {
                DateFrom = DateTime.Now.ToString("yyyy-MM-dd"),
                DateTo = DateTime.Now.AddYears(5).ToString("yyyy-MM-dd"),
                TeamId = "8CAE50DDB19FBD65D94AE2555AC52F40",
                PersonId = Guid.NewGuid().ToString(),
                Salary = random.Next(100000, 1000000)
            };
        }
    }
}