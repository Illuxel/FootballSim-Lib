using DatabaseLayer.Enums;
using System;

namespace DatabaseLayer.Services
{
    public class PlayerGameData
    {
        public string Id { get; private set; }
        public string PlayerName { get; set; }
        public string PlayerSurname { get; set; }
        public string ClubId { get; set; }
        public string RealDate { get; set; }
        public string GameDate { get; set; }
        public double Money { get; set; }
        public UserRole Role { get; set; }

        public PlayerGameData(string playerName,string playerSurname, string clubId, string realDate, string gameDate, double money, UserRole role)
        {
            this.Id = Guid.NewGuid().ToString();
            this.PlayerName = playerName;
            this.PlayerSurname = playerSurname;
            this.ClubId = clubId;
            this.RealDate = realDate;
            this.GameDate = gameDate;
            this.Money = money;           
            this.Role = role;
        }
        public PlayerGameData()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
