using System;
using System.IO;
using System.Collections.Generic;

using DatabaseLayer.Settings;
using DatabaseLayer.Enums;
using DatabaseLayer.Repositories;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DatabaseLayer.Services
{
    public class SaveInfo : IComparable<SaveInfo>
    {
        public string SaveName { get; set; }
        public DateTime SaveDate { get; set; }
        public string SavePath 
        {
            get 
            {
                return Path.Combine(SavesSettings.SavesFolderPath, SaveName);
            }
        }

        public PlayerGameData PlayerData { get; set; }

        public int CompareTo(SaveInfo other)
        {
            return DateTime.Compare(other.SaveDate, SaveDate);
        }
    }

    public class SavesManager
    {
        private static SavesManager _instance;
        private readonly List<SaveInfo> _saves;

        private SavesManager() 
        {
            _saves = new List<SaveInfo>();

            // check if GameSaves folder exists
            if (!Directory.Exists(SavesSettings.SavesFolderPath))
            {
                Directory.CreateDirectory(SavesSettings.SavesFolderPath);
            }

            loadSaves();
        }

        public static SavesManager GetInstance()
        {
            if (_instance == null)
            { 
                _instance = new SavesManager();
            }

            return _instance;
        }

        /// <summary>
        /// Returns all saves
        /// </summary>
        /// <returns></returns>
        public List<SaveInfo> GetAllSaves()
        { 
            return _saves;
        }

        /// <summary>
        /// Loads latest available game save
        /// </summary>
        /// <returns>Returns available save or null if no saves</returns>
        public SaveInfo Continue()
        {
            if (_saves.Count == 0)
            { 
                return null;
            }

            var latestSave = _saves[0]; // faster than .First()

            SavesSettings.SetSaveFolderName(latestSave.SaveName); 

            return latestSave;
        }

        /// <summary>
        /// Creates new save with specified name saves and returns it
        /// </summary>
        /// <returns>Returns new save</returns>
        public SaveInfo NewGame(PlayerGameData playerData, string saveName)
        {
            SavesSettings.SetSaveFolderName(saveName);

            var existedSave = getSave(saveName);

            if (existedSave != null)
            {
                return existedSave;
            }

            // creating new folder for database
            Directory.CreateDirectory(DatabaseSettings.DBFolderPath);
            // copying database to a new save folder
            File.Copy(DatabaseSettings.BaseDBPath, DatabaseSettings.DBFilePath, true);

            // updating database with a new player
            var person = new Person()
            {
                Name = playerData.PlayerName,
                Surname = playerData.PlayerSurname,
                Birthday = DateTime.MinValue,
                CurrentRoleID = (int)UserRole.Scout,
                CountryID = 0
            };
            var isUpdated = new PersonRepository().Insert(person);

            if (!isUpdated)
            {
                Directory.Delete(SavesSettings.CurrentSaveFolderPath, true);
                SavesSettings.SetSaveFolderName(null);

                return null;
            }

            var saveInfo = new SaveInfo
            {
                SaveName = saveName,
                SaveDate = DateTime.Now,
                PlayerData = playerData
            };

            // saving new data
            Save(saveInfo);

            _saves.Add(saveInfo);
            _saves.Sort();

            return saveInfo;
        }

        /// <summary>
        /// Updates game progress in specified save. 
        /// If there is no save creates new save (like new game)
        /// </summary>
        public void Save(SaveInfo saveInfo)
        {
            var saveFolderPath = Path.Combine(SavesSettings.SavesFolderPath, saveInfo.SaveName);
            var saveFolderInfo = new DirectoryInfo(saveFolderPath);

            if (!saveFolderInfo.Exists)
            {
                NewGame(saveInfo.PlayerData, saveInfo.SaveName);
            }
            else
            {
                saveFolderInfo.LastWriteTime = DateTime.Now;
                var jsonUserGameData = JsonConvert.SerializeObject(saveInfo.PlayerData);

                File.WriteAllText(SavesSettings.CurrentUserGameDataPath, jsonUserGameData);
                SavesSettings.SetSaveFolderName(saveFolderInfo.Name);
            }
        }

        /// <summary>
        /// Loads available save by name
        /// </summary>
        /// <returns>Returns available save</returns>
        public SaveInfo Load(string saveName)
        {
            var saveInfo = getSave(saveName);

            if (saveInfo == null) 
            {
                return saveInfo;
            }

            SavesSettings.SetSaveFolderName(saveName);

            return saveInfo;
        }

        /// <summary>
        /// Removes available save from game saves
        /// </summary>
        /// <returns>Returns <see langword="true"/> when delition was successfull</returns>
        public bool Delete(string saveName)
        {
            var saveFolder = Path.Combine(SavesSettings.SavesFolderPath, saveName);

            SavesSettings.SetSaveFolderName(null);

            try
            {
                Directory.Delete(saveFolder, true);

                var saveInfo = getSave(saveName);

                if (saveInfo != null)
                {
                    _saves.Remove(saveInfo);
                    _saves.Sort();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Reloads cached saves in memory
        /// </summary>
        public void ReloadSaves()
        {
            SavesSettings.SetSaveFolderName(null);
            _saves.Clear();
            loadSaves();
        }

        private void loadSaves()
        {
            var savesPaths = Directory.GetDirectories(SavesSettings.SavesFolderPath);

            foreach (var savePath in savesPaths)
            {
                var saveInfo = new SaveInfo()
                {
                    SaveName = Path.GetFileNameWithoutExtension(savePath),
                    SaveDate = Directory.GetLastWriteTime(savePath),
                };

                var jsonUserGameDataPath = Path.Combine(savePath, SavesSettings.UserGameDataFileName);

                try
                { 
                    var jsonUserDataFile = File.ReadAllText(jsonUserGameDataPath);
                    var jsonUserData = JObject.Parse(jsonUserDataFile);

                    var playerData = jsonUserData.ToObject<PlayerGameData>();

                    if (playerData == null) 
                    {
                        continue;
                    }

                    saveInfo.PlayerData = playerData;
                }
                catch (Exception)
                {
                    continue;
                }

                _saves.Add(saveInfo);
            }

            _saves.Sort();
        }

        private SaveInfo getSave(string saveName)
        {
            if (_saves.Count == 0)
            {
                return null;
            }

            try
            {
                return _saves.Find(saveData => saveData.SaveName == saveName);
            }
            catch (ArgumentNullException) 
            {
                return null;
            }
        }
    }
}
