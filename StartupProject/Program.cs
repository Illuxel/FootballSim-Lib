using BusinessLogicLayer.Scenario;
using BusinessLogicLayer.Services;
using DatabaseLayer.Services;
using System.IO;
using System;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var settings = new GenerateGameActionsToNextMatchSettings(Directory.GetCurrentDirectory());

            var playerGameData = new PlayerGameData();
            playerGameData.GameDate = new DateTime(2023, 8, 12).ToString("yyyy-MM-dd");
            playerGameData.RealDate = DateTime.Now.ToString("yyyy-MM-dd");
            playerGameData.CurrentLevel = DatabaseLayer.Enums.ScoutSkillLevel.Level4;
            playerGameData.ClubId = "015834FD9556AAEC44DE54CDE350235B";
            var saveInfo = new SaveInfo(playerGameData, "test1");

            new GenerateGameActionsToNextMatch(saveInfo, settings).SimulateActions();
        }
    }
}