using FootBalLife.Database;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DatabaseLayer.DBSettings;

namespace FootBalLife.LoadGameManager
{
    public class PlayerData
    {
        public string Id { get; set; }
        public string PlayerName { get; set; } = "";

        public string Date { get; set; } = "";

        public double Money { get; set; } = 0.0;

        public PlayerData(string Id, string PlayerName, string Date, double Money)
        {
            this.Id = Id;
            this.PlayerName = PlayerName;
            this.Money = Money;
            this.Date = Date;
        }
    }

    public class SaveInfoJson
    {
        public string PathForDataBase { get; set; }

        public SaveInfoJson(string PathForDataBase)
        {
            this.PathForDataBase = PathForDataBase;
        }
    }

    public class SaveInfo
    {
        public PlayerData PlayerData { get; set; }
        public string DbLocalPath { get; set; }
        public string SaveName { get; set; }
        
        public void Show()
        {
            Console.WriteLine("ConnectionString: " + DbLocalPath + ". User data: " + PlayerData.Date + " Save name "+ SaveName);
        }

        public SaveInfo(PlayerData data, string connectionString) { this.PlayerData = data; this.DbLocalPath = connectionString; }
        public SaveInfo() { }
    }

    public class LoadGameManager
    {
        public static LoadGameManager? instance;
        private LoadGameManager(string pathToSave)
        {
            DatabaseManager._pathToSave = pathToSave;
            DatabaseManager._savePathInfo = Path.Combine(pathToSave, "SavesInfo.json");
        }

        public static LoadGameManager GetInstance(string pathToSave = "")
        {
            if (instance == null)
            {
                instance = new LoadGameManager(pathToSave);
                
            }
            return instance;
        }

        public List<SaveInfo> GetAllSaves()
        {
            List<SaveInfo> result = new List<SaveInfo>();
            DirectoryInfo dir = new DirectoryInfo(DatabaseManager._pathToSave);
            DirectoryInfo[] info = dir.GetDirectories("*.*");
            foreach (DirectoryInfo d in info)
            {
                string path = Path.Combine(DatabaseManager._pathToSave, d.Name, DatabaseManager._userDataFileName);
                string json = File.ReadAllText(path);
                PlayerData someData = JsonConvert.DeserializeObject<PlayerData>(json);
                SaveInfo saveInfo = new SaveInfo() { PlayerData = someData, DbLocalPath = Path.Combine(DatabaseManager._pathToSave, d.Name), SaveName = d.Name };
                result.Add(saveInfo);
            }
            return result;
        }
        public bool Delete(string saveName)
        {
            //Get Save for deleting
            DirectoryInfo dir = new DirectoryInfo(DatabaseManager._pathToSave);
            var saveDirectory = dir.GetDirectories($"{saveName}.*").FirstOrDefault();

            //Deleting save
            Directory.Delete(saveDirectory.FullName, true);

            //Calculating loading save for Json SavesInfo
            DirectoryInfo[] info = dir.GetDirectories("*.*");
            var lastModifySaveName = string.Empty;
            var lastModifySaveDate = DateTime.MinValue;
            foreach (DirectoryInfo saveFolder in info)
            {
                var lastModifyDateTime = File.GetLastWriteTime(Path.Combine(DatabaseManager._pathToSave, saveFolder.Name));
                if (lastModifyDateTime > lastModifySaveDate)
                {
                    lastModifySaveName = saveFolder.Name;
                }
            }

            SaveInfoJson dataToJSON = new SaveInfoJson(Path.Combine(DatabaseManager._pathToSave, lastModifySaveName));
            File.WriteAllText(DatabaseManager._savePathInfo, JsonConvert.SerializeObject(dataToJSON, Formatting.Indented));


            return true;
        }

        public SaveInfo? Continue()
        {
            //getting directory
            SaveInfo saveInfo = new SaveInfo();
            string jsonSave = File.ReadAllText(DatabaseManager._savePathInfo);
            string savePath = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonSave).ElementAt(0).Value.ToString();
            if (!Directory.Exists(savePath))
            {
                return null;
            }

            //Create connection to database
            DatabaseManager.SetConnectionString(savePath);

            //Filling class with data
            string json = File.ReadAllText(Path.Combine(savePath, DatabaseManager._userDataFileName));
            PlayerData someData = JsonConvert.DeserializeObject<PlayerData>(json);
            saveInfo.SaveName = savePath.Substring(savePath.LastIndexOf("\\") + 1, savePath.Length - savePath.LastIndexOf("\\") - 1);
            saveInfo.PlayerData = someData;
            saveInfo.DbLocalPath = Path.Combine(savePath, DatabaseManager._originalDbFileName);

            
            return saveInfo;
        }
        public SaveInfo? Load(string SaveName)
        {
            //Validate of Exist directory
            string savePath = Path.Combine(DatabaseManager._pathToSave, SaveName);
            var saveInfo = new SaveInfo();
            if (!Directory.Exists(savePath))
            {
                return null;
            }

            //Refresh Json SaveInfo
            SaveInfoJson dataToJSON = new SaveInfoJson(savePath);
            File.WriteAllText(DatabaseManager._savePathInfo, JsonConvert.SerializeObject(dataToJSON, Formatting.Indented));

            //Create connection to database
            DatabaseManager.SetConnectionString(savePath);

            //getting data
            savePath = Path.Combine(savePath, DatabaseManager._userDataFileName);
            string json = File.ReadAllText(savePath);
            PlayerData someData = JsonConvert.DeserializeObject<PlayerData>(json);
            
            saveInfo.SaveName = SaveName;
            saveInfo.PlayerData = someData;
            saveInfo.DbLocalPath = Path.Combine(savePath, DatabaseManager._originalDbFileName);

            

            return saveInfo;
        }

        public SaveInfo? NewGame(PlayerData data, string saveName = "")
        {

            var saveInfo = new SaveInfo();
            //Crete Save File
            string path;
            if (string.IsNullOrEmpty(saveName))
            {
                var listSaves = GetFilesInfo();
                path = getNewSavePath(listSaves, saveName);
            }
            else
            {
                path = getNewSavePath(null, saveName);
            }

            if (string.IsNullOrEmpty(path))
            {
                return null;
            }


            //Create Save Info File
            SaveInfoJson dataToJSON = new SaveInfoJson(path);
            File.WriteAllText(DatabaseManager._savePathInfo, JsonConvert.SerializeObject(dataToJSON, Formatting.Indented));

            

            //Filling the database with data
            Directory.CreateDirectory(path);
            File.Copy(DatabaseManager._originalDbFilePath, Path.Combine(path, DatabaseManager._originalDbFileName));
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var jsonpath = Path.Combine(path, DatabaseManager._userDataFileName);
            File.WriteAllText(jsonpath, json);

            //Create connection to database
            DatabaseManager.SetConnectionString(path);

            //Filling the class SaveInfo with data
            saveInfo.SaveName = saveName;
            saveInfo.PlayerData = data;
            saveInfo.DbLocalPath = Path.Combine(path, DatabaseManager._originalDbFileName);
            return saveInfo;
        }


        private string getNewSavePath(List<string> listSaves, string SaveName)
        {
            if (SaveName != "")
            {
                string result = DatabaseManager._pathToSave + "\\" + SaveName;
                if (Directory.Exists(result))
                {
                    throw new Exception("Save with this name is exist");
                }
                return result;
            }
            var saveNumber = 0;
            if (listSaves.Count == 0)
            {
                saveNumber = 1;
            }
            else
            {
                var saves = listSaves.OrderBy(x => x);
                var saveNumberString = Regex.Match(saves.Last(), @"\d+");
                if (!Int32.TryParse(string.Join("", saveNumberString), out saveNumber))
                {
                    return string.Empty;
                }
                ++saveNumber;
            }

            return Path.Combine(DatabaseManager._pathToSave, "Save" + saveNumber);
        }

        private List<string> GetFilesInfo()
        {
            var list = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(DatabaseManager._pathToSave);
            DirectoryInfo[] info = dir.GetDirectories("*Save*.*");
            foreach (DirectoryInfo file in info)
            {
                list.Add(file.Name);
            }

            return list;
        }
    }
}
