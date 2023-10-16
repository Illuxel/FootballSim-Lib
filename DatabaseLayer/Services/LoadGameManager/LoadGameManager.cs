using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DatabaseLayer.DBSettings;
using DatabaseLayer.Repositories;

namespace DatabaseLayer.Services
{
    public class SaveInfoJson
    {
        public string PathForDataBase { get; set; }

        public SaveInfoJson(string pathForDataBase)
        {
            this.PathForDataBase = pathForDataBase;
        }
    }

    public class SaveInfo
    {
        public PlayerGameData PlayerData { get; set; }
        public string DbLocalPath { get; set; }
        public string SaveName { get; set; }

        internal string GetSavePath()
        {
            return Path.Combine(LoadGameManagerSettings.BasePath, LoadGameManager.SavesFolderName, SaveName);
        }
        public SaveInfo(PlayerGameData data, string saveName)
        {
            PlayerData = data;
            SaveName = saveName;
            DbLocalPath = Path.Combine(GetSavePath(), DatabaseManager.OriginalDbFileName);
        }
        internal SaveInfo() { }
    }

    internal static class LoadGameManagerSettings
    {
        public static string BasePath { get; internal set; }
    }

    public class LoadGameManager
    {
        internal static string SavesFolderName = "gameSaves";
        public static LoadGameManager instance;
        private PersonRepository _personRepository;

        private LoadGameManager()
        {
            _personRepository = new PersonRepository();

            if (!LoadGameManagerSettings.BasePath.Contains(SavesFolderName))
            {
                DatabaseManager.PathToSave = Path.Combine(LoadGameManagerSettings.BasePath, SavesFolderName);
            }
            else
            {
                DatabaseManager.PathToSave = LoadGameManagerSettings.BasePath;
            }
            DatabaseManager.SavePathInfo = Path.Combine(LoadGameManagerSettings.BasePath, "SavesInfo.json");
        }

        public static LoadGameManager GetInstance()
        {
            if (instance == null)
            {
                if(string.IsNullOrEmpty(LoadGameManagerSettings.BasePath))
                {
                    LoadGameManagerSettings.BasePath = Directory.GetCurrentDirectory();
                }
                instance = new LoadGameManager();

            }
            return instance;
        }

        public List<SaveInfo> GetAllSaves()
        {
            List<SaveInfo> result = new List<SaveInfo>();
            DirectoryInfo dir = new DirectoryInfo(DatabaseManager.PathToSave);
            DirectoryInfo[] info = dir.GetDirectories("*.*");
            foreach (DirectoryInfo d in info)
            {
                string path = Path.Combine(DatabaseManager.PathToSave, d.Name, DatabaseManager.UserDataFileName);
                string json = File.ReadAllText(path);
                PlayerGameData someData = JsonConvert.DeserializeObject<PlayerGameData>(json);
                SaveInfo saveInfo = new SaveInfo() { PlayerData = someData, DbLocalPath = Path.Combine(DatabaseManager.PathToSave, d.Name), SaveName = d.Name };
                result.Add(saveInfo);
            }
            return result;
        }
        public bool Delete(string saveName)
        {
            //Get Save for deleting
            DirectoryInfo dir = new DirectoryInfo(DatabaseManager.PathToSave);
            var saveDirectory = dir.GetDirectories($"{saveName}.*").FirstOrDefault();

            //Deleting save
            Directory.Delete(saveDirectory.FullName, true);

            //Calculating loading save for Json SavesInfo
            DirectoryInfo[] info = dir.GetDirectories("*.*");
            var lastModifySaveName = string.Empty;
            var lastModifySaveDate = DateTime.MinValue;
            foreach (DirectoryInfo saveFolder in info)
            {
                var lastModifyDateTime = File.GetLastWriteTime(Path.Combine(DatabaseManager.PathToSave, saveFolder.Name));
                if (lastModifyDateTime > lastModifySaveDate)
                {
                    lastModifySaveName = saveFolder.Name;
                }
            }

            SaveInfoJson dataToJSON = new SaveInfoJson(Path.Combine(DatabaseManager.PathToSave, lastModifySaveName));
            File.WriteAllText(DatabaseManager.SavePathInfo, JsonConvert.SerializeObject(dataToJSON, Formatting.Indented));


            return true;
        }

        public void SaveGame(SaveInfo saveInfo)
        {
            updateUserData(saveInfo);
        }

        public SaveInfo Continue()
        {
            //getting directory
            SaveInfo saveInfo = new SaveInfo();
            string jsonSave = File.ReadAllText(DatabaseManager.SavePathInfo);
            string savePath = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonSave).ElementAt(0).Value.ToString();
            if (!Directory.Exists(savePath))
            {
                return null;
            }

            //Create connection to database
            DatabaseManager.SetConnectionString(savePath);

            //Filling class with data
            string json = File.ReadAllText(Path.Combine(savePath, DatabaseManager.UserDataFileName));
            PlayerGameData someData = JsonConvert.DeserializeObject<PlayerGameData>(json);
            saveInfo.SaveName = savePath.Substring(savePath.LastIndexOf("\\") + 1, savePath.Length - savePath.LastIndexOf("\\") - 1);
            saveInfo.PlayerData = someData;
            saveInfo.DbLocalPath = Path.Combine(savePath, DatabaseManager.OriginalDbFileName);


            return saveInfo;
        }
        public SaveInfo Load(string saveName)
        {
            //Validate of Exist directory
            string savePath = Path.Combine(DatabaseManager.PathToSave, saveName);
            var saveInfo = new SaveInfo();
            if (!Directory.Exists(savePath))
            {
                return null;
            }

            //Refresh Json SaveInfo
            SaveInfoJson dataToJSON = new SaveInfoJson(savePath);
            File.WriteAllText(DatabaseManager.SavePathInfo, JsonConvert.SerializeObject(dataToJSON, Formatting.Indented));

            //Create connection to database
            DatabaseManager.SetConnectionString(savePath);

            //getting data
            savePath = Path.Combine(savePath, DatabaseManager.UserDataFileName);
            string json = File.ReadAllText(savePath);
            PlayerGameData someData = JsonConvert.DeserializeObject<PlayerGameData>(json);

            saveInfo.SaveName = saveName;
            saveInfo.PlayerData = someData;
            saveInfo.DbLocalPath = Path.Combine(savePath, DatabaseManager.OriginalDbFileName);



            return saveInfo;
        }

        public SaveInfo NewGame(PlayerGameData data, string saveName = "")
        {
            var saveInfo = new SaveInfo();
            //Crete Save File
            string path;
            if (string.IsNullOrEmpty(saveName))
            {
                var listSaves = GetFilesInfo();
                path = getNewSavePath(listSaves, ref saveName);
            }
            else
            {
                path = getNewSavePath(null, ref saveName);
            }

            if (string.IsNullOrEmpty(path))
            {
                return null;
            }


            //Create Save Info File
            SaveInfoJson dataToJSON = new SaveInfoJson(path);
            File.WriteAllText(DatabaseManager.SavePathInfo, JsonConvert.SerializeObject(dataToJSON, Formatting.Indented));

            insertNewPerson(data);

            //Filling the class SaveInfo with data
            saveInfo.SaveName = saveName;
            saveInfo.PlayerData = data;
            saveInfo.DbLocalPath = Path.Combine(path, DatabaseManager.OriginalDbFileName);

            //Filling the database with data
            Directory.CreateDirectory(path);
            File.Copy(DatabaseManager.OriginalDbFilePath, Path.Combine(path, DatabaseManager.OriginalDbFileName));
            updateUserData(saveInfo);

            //Create connection to database
            DatabaseManager.SetConnectionString(path);

            return saveInfo;
        }

        private void insertNewPerson(PlayerGameData data)
        {
            var person = new Person();

            person.Name = data.PlayerName;
            person.Surname = data.PlayerSurname;
            person.CurrentRoleID = (int)Enums.UserRole.Scout;
            person.Birthday = DateTime.MinValue;
            person.CountryID = 0;

            if (_personRepository.Insert(person))
            {
                data.PersonId = person.Id;
            }
        }

        private void updateUserData(SaveInfo saveInfo )
        {
            var json = JsonConvert.SerializeObject(saveInfo.PlayerData, Formatting.Indented);
            var jsonpath = Path.Combine(saveInfo.GetSavePath(), DatabaseManager.UserDataFileName);
            File.WriteAllText(jsonpath, json);
        }

        private string getNewSavePath(List<string> listSaves, ref string saveName)
        {
            if (!string.IsNullOrEmpty(saveName))
            {
                string result = DatabaseManager.PathToSave + "\\" + saveName;
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
                if (!int.TryParse(string.Join("", saveNumberString), out saveNumber))
                {
                    return string.Empty;
                }
                ++saveNumber;
            }
            saveName = "Save" + saveNumber;
            return Path.Combine(DatabaseManager.PathToSave, saveName);
        }

        private List<string> GetFilesInfo()
        {
            var list = new List<string>();
            if(!Directory.Exists(DatabaseManager.PathToSave))
            {
                Directory.CreateDirectory(DatabaseManager.PathToSave);
            }
            DirectoryInfo dir = new DirectoryInfo(DatabaseManager.PathToSave);
            var info = dir.GetDirectories("*Save*.*");
            foreach (DirectoryInfo file in info)
            {
                list.Add(file.Name);
            }

            return list;
        }
    }
}
