using System;

using DatabaseLayer.Settings;
using DatabaseLayer.Services;
using DatabaseLayer.Repositories;

using BusinessLogicLayer.Services;

namespace StartupProject
{
    internal class SavesManagerExample
    {
        public static void Main(string[] args)
        {
            var savesManager = SavesManager.GetInstance();

            const string playerName = "Illia";
            const string playerSurname = "Sam";
            const string teamName = "Brentford";

            var teams = new TeamRepository().Retrieve();
            var team = teams.Find(team => team.ExtName == teamName);

            var startDate = new SeasonValueCreator().GetSeasonStartDate(DateTime.Now.Year).ToString("dd.MM.yyyy");

            bool exit = false;
            SaveInfo saveInfo = null;

            while (!exit)
            {
                Console.Write("Enter command: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "getall":

                        var saves = savesManager.GetAllSaves();

                        if (saves.Count != 0)
                        {
                            foreach (var save in saves)
                            {
                                Console.WriteLine($"Save {save.SaveName} - {save.SaveDate}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("You dont have any saves");
                        }

                        break;
                    case "continue":

                        saveInfo = savesManager.Continue();

                        if (saveInfo != null)
                        {
                            Console.WriteLine($"Loaded latest save {saveInfo.SaveName} ({saveInfo.PlayerData.Money}$) - {saveInfo.SaveDate}");
                        }
                        else
                        {
                            Console.WriteLine("Could not load save. Maybe you dont have any saves");
                        }

                        break;
                    case "load":

                        Console.Write("Enter save name: ");
                        input = Console.ReadLine();

                        saveInfo = savesManager.Load(input);

                        if (saveInfo != null)
                        {
                            Console.WriteLine($"Loaded latest save {saveInfo.SaveName} ({saveInfo.PlayerData.Money}$) - {saveInfo.SaveDate}");
                        }
                        else
                        {
                            Console.WriteLine("Could not load save. Maybe you dont have any saves");
                        }

                        break;
                    case "newgame":

                        Console.Write("Enter money amount: ");
                        input = Console.ReadLine();

                        var userGameData = new PlayerGameData()
                        {
                            ClubId = team.Id,
                            PlayerName = playerName,
                            PlayerSurname = playerSurname,
                            Money = Convert.ToDouble(input),
                            GameDate = startDate,
                            Role = 0,
                            CurrentLevel = 0
                        };

                        Console.Write("Enter save name: ");
                        input = Console.ReadLine();

                        saveInfo = savesManager.NewGame(userGameData, input);

                        if (saveInfo != null)
                        {
                            Console.WriteLine($"Created save {saveInfo.SaveName} ({saveInfo.PlayerData.Money}$) - {saveInfo.SaveDate}");
                        }
                        else
                        {
                            Console.WriteLine($"There is an issue with creating new save. Check repo Person or set base game path");
                        }

                        break;
                    case "save":

                        Console.Write("Enter money amount: ");
                        input = Console.ReadLine();

                        saveInfo.PlayerData.Money = Convert.ToInt32(input);

                        savesManager.SaveGame(saveInfo);
                        saveInfo = savesManager.Continue();

                        Console.WriteLine($"Last save new money {saveInfo.PlayerData.Money}");

                        break;
                    case "delete":

                        Console.Write("Enter save name: ");
                        input = Console.ReadLine();

                        var isDeleted = savesManager.Delete(input);

                        if (isDeleted)
                        {
                            Console.WriteLine($"Deleted save {input}");
                        }
                        else
                        {
                            Console.WriteLine("Could not delete save");
                        }

                        break;
                    case "reload":
                        savesManager.ReloadSaves();
                        break;
                    case "exit":
                        exit = true;
                        break;
                };

                Console.WriteLine($"\tGame dirs info\nCurrent save path: {SavesSettings.CurrentSaveFolderPath}\nDatabase path: {DatabaseSettings.DBFilePath}");

                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}