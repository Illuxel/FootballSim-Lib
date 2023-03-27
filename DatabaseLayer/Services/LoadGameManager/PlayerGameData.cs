using System;

namespace DatabaseLayer.Services
{
    public class PlayerGameData
    {
        public string Id { get; set; }
        public string PlayerName { get; set; }

        public string Date { get; set; }

        public double Money { get; set; }

        public PlayerGameData(string Id, string PlayerName, string Date, double Money)
        {
            this.Id = Id;
            this.PlayerName = PlayerName;
            this.Money = Money;
            this.Date = Date;
        }
        public PlayerGameData()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
