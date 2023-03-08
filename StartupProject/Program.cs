using FootBalLife.Database.Repositories;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;

using FootBalLife.Database.Repositories;
using DatabaseLayer.Services;
using System;
using System.Collections.Generic;
using Services.PersonNameGenaration;

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

                    PersonNameGenaration nameGeneration = new PersonNameGenaration();

                    foreach (var one in nameGeneration.CreatePersonNames(9, 30))
                    {
                        Console.WriteLine("Name: " + one.Name + ". Surname: " + one.Surname);
                    }

                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();





                    var cmc = new ScheduleMatchGenerator();
                    cmc.Generate(DateTime.Now.Year);






                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    Console.WriteLine($"Час виконання коду: {ts}");

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
