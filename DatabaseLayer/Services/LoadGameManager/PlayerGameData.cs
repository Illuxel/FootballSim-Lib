using System;

namespace DatabaseLayer.Services
{
    public class PlayerGameData
    {
        public string Id { get; private set; }
        public string PlayerName { get; set; }
        public string PlayerSurname { get; set; }
        public string Club { get; set; }
        public string Date { get; set; }

        public double Money { get; set; }

        public PlayerGameData(string playerName,string playerSurname, string club, string date, double money)
        {
            this.Id = Guid.NewGuid().ToString();
            this.PlayerName = playerName;
            this.PlayerSurname = playerSurname;
            this.Club = club;
            this.Money = money;
            this.Date = date;
        }
        public PlayerGameData()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
