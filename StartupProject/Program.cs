using BusinessLogicLayer.Scenario;
using BusinessLogicLayer.Services;
using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var gTour1 = new GenerateAllMatchesByTour(new System.DateTime(2023, 08, 12));
            gTour1.Generate();
            var gTour2 = new GenerateAllMatchesByTour(new System.DateTime(2023, 08, 19));
            gTour2.Generate();
            var gTour3 = new GenerateAllMatchesByTour(new System.DateTime(2023, 08, 26));
            gTour3.Generate();

            var teamId = "678065FDDB06C590A0D0F9EDC2B5196F";
            var playerUpdate = new PlayerSkillsTrainer();
            playerUpdate.TrainPlayers(teamId, DatabaseLayer.Enums.TrainingMode.AdvancedForLastGameBench);
        }
    }
}
