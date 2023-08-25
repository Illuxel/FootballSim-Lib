using BusinessLogicLayer.Scenario;
using BusinessLogicLayer.Services;
using DatabaseLayer.Enums;
using DatabaseLayer.Services;
using System;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var data = new PlayerGameData()
            {
                CurrentLevel = ScoutSkillLevel.Level2,
                Money = 1000000,
                Role = UserRole.Scout,
                PlayerName = "Test",
                PlayerSurname = "Test",
                RealDate = DateTime.Now.ToString("yyyy-MM-dd"),
                GameDate = "2023-01-01",
                ClubId = "656BD8A423624CD8D63235FCC8067E89",
            };
            var instance = LoadGameManager.GetInstance();
            var save = instance.Load("1");
            if (save == null)
            {
                instance.NewGame(data, "1");
                save = instance.Load("1");
            }
            var playerData = save.PlayerData;


            var service = new AccessedLeagues();
            var team = service.DefineAccessedLeagues(playerData);
            foreach (var item in team)
            {
                Console.WriteLine(item.Name);
                Console.WriteLine(item.Id);
                Console.WriteLine(item.CurrentRating);
            }
        }
    }
}
