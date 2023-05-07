using DatabaseLayer;
using System;
using System.Drawing;
using BusinessLogicLayer;
using BusinessLogicLayer.Services;
using DatabaseLayer.Repositories;
using DatabaseLayer.Services;
using System.IO;

namespace StartupProject
{
    internal class Program
    {
       
        public static void Main(string[] args)
        {

            var instance = LoadGameManager.GetInstance(Directory.GetCurrentDirectory());

            SaveInfo saveInfo = new SaveInfo(new PlayerGameData()
            {
                //Date = DateTime.Now.ToString(),
                PlayerName = "pl1",
                Money = 50,

            }, "Save5");

            instance.SaveGame(saveInfo);
            /*var mge = new MatchGeneratingExampleUsing();
            mge.GenerateMatch();*/
        }
    }
}
