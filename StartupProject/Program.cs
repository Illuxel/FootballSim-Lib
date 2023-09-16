using BusinessLogicLayer.Scenario;
using BusinessLogicLayer.Services;
using DatabaseLayer;
using DatabaseLayer.Enums;
using DatabaseLayer.Repositories;
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
                CurrentLevel = ScoutSkillLevel.Level3,
                Money = 1000000,
                Role = UserRole.Scout,
                PlayerName = "Test",
                PlayerSurname = "Test",
                RealDate = DateTime.Now.ToString("yyyy-MM-dd"),
                GameDate = "2023-08-12",
                ClubId = "656BD8A423624CD8D63235FCC8067E89",
            };
            var instance = LoadGameManager.GetInstance();
            var save = instance.Load("1");
            if (save == null)
            {
                instance.NewGame(data, "1");
                save = instance.Load("1");
            }

            var team = "test1";
            new MatchRepository().Insert(new Match()
            {
                Id = "228",
                GuestTeamId = team,
                HomeTeamId = team,
                TourNumber = 4,
                MatchDate = "2023-08-12",
                IsPlayed = false,
            });
            new PlayerInMatchRepository().Insert(new PlayerInMatch()
            {
                Id = Guid.NewGuid().ToString(),
                MatchId = "228",
                PlayerId = "ffcf1c95-a9fc-4e51-ba7d-98cc0f63327e",
                TeamId = team,
            });

            new GoalRepository().Insert(new Goal()
            {
                Id = Guid.NewGuid().ToString(),
                MatchId = "228",
                PlayerId = "ffcf1c95-a9fc-4e51-ba7d-98cc0f63327e",
                TeamId = team,
                AssistPlayerId = ""

            });

            /*var gen = new GenerateGameActionsToNextMatch(save);
            gen.SimulateActions();
            save.PlayerData.GameDate = DateTime.Parse(save.PlayerData.GameDate).AddDays(7).ToString("yyyy-MM-dd");

            gen = new GenerateGameActionsToNextMatch(save);
            gen.SimulateActions();
            save.PlayerData.GameDate = DateTime.Parse(save.PlayerData.GameDate).AddDays(7).ToString("yyyy-MM-dd");

            gen = new GenerateGameActionsToNextMatch(save);
            gen.SimulateActions();
            save.PlayerData.GameDate = DateTime.Parse(save.PlayerData.GameDate).AddDays(7).ToString("yyyy-MM-dd");

            var goalAssistTracker = new GoalAssistTracker();
            var topGoalScorers = goalAssistTracker.GetTopGoalScorers("2023/2024", 1, 10);
            var topAssists = goalAssistTracker.GetTopAssists("2023/2024", 1, 10);*/
        }
    }
}