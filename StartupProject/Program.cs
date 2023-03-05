using FootBalLife.Database.Repositories;
using FootBalLife.LoadGameManager;
using System;
using System.Collections.Generic;
using DatabaseLayer.DBSettings;

namespace StartupProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choise operation!");
            string ch = Console.ReadLine();
            var guidString = Guid.NewGuid().ToString();
            PlayerData someData = new PlayerData(guidString, "Mikola", "21/01/2023", 1991.1);
            var fileManager = LoadGameManager.GetInstance("D:\\SaveFolder");

            SaveInfo saveInfo = new SaveInfo();


            switch (ch)
            {
                case "1":
                    saveInfo = fileManager.Continue();
                    saveInfo.Show();

                    TeamRepository teamRepository1 = new TeamRepository();
                    var teams = teamRepository1.Retrive();
                    foreach (var item in teams)
                    {
                        Console.WriteLine(item.Name);
                    }
                    break;
                case "2":
                    Console.WriteLine("Choise Name!");
                    fileManager.Load(Console.ReadLine()).Show();


                    TeamRepository teamRepository2 = new TeamRepository();
                    var teams2 = teamRepository2.Retrive();
                    foreach (var item in teams2)
                    {
                        Console.WriteLine(item.Name);
                    }
                    break;

                case "3":
                    Console.WriteLine("Create Name!");
                    fileManager.NewGame(someData, Console.ReadLine()).Show();

                    TeamRepository teamRepository3 = new TeamRepository();
                    var teams3 = teamRepository3.Retrive();
                    foreach (var item in teams3)
                    {
                        Console.WriteLine(item.Name);
                    }

                    break;
                case "4":
                    Console.WriteLine("Choise Name!");
                    fileManager.Delete(Console.ReadLine());
                    break;

                case "5":
                    List<SaveInfo> data = fileManager.GetAllSaves();
                    foreach (SaveInfo save in data)
                    {
                        save.Show();
                    }
                    break;
            }

        }

    }
}
