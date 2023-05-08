using DatabaseLayer;
using System;
using System.Drawing;
using BusinessLogicLayer;
using BusinessLogicLayer.Services;
using DatabaseLayer.Repositories;
using DatabaseLayer.Services;
using System.IO;
using System.Linq;

namespace StartupProject
{
    internal class Program
    {
       
        public static void Main(string[] args)
        {
/*
            var instance = LoadGameManager.GetInstance(Directory.GetCurrentDirectory());

            SaveInfo saveInfo = new SaveInfo(new PlayerGameData()
            {
                //Date = DateTime.Now.ToString(),
                PlayerName = "pl1",
                Money = 50,

            }, "Save5");

            instance.SaveGame(saveInfo);*/

            /*var mge = new MatchGeneratingExampleUsing();
            mge.GenerateMatch();*/
            TeamForMatchCreator TeamCreator = new TeamForMatchCreator();
            TeamRepository teamRepository = new TeamRepository();
            var team = teamRepository.Retrive("015834FD9556AAEC44DE54CDE350235B");
            var match = TeamCreator.Create(team);
            match.SubstitutePlayer(0, match.SparePlayers[0]);
            var spared = match.SparedPlayers[0];
            var squad = match.MainPlayers;
        }
    }
}
