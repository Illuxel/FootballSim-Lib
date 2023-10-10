using System.IO;

namespace DatabaseLayer.Settings
{
    public static class SavesSettings
    {
        public const string SavesFolderName = "GameSaves";
        public const string UserGameDataFileName = "UserGameData.json";

        private static string _currentSaveFolderName;

        /// <summary>
        /// Sets current game save folder name to be used
        /// </summary>
        public static void SetSaveFolderName(string folderName)
        {
            _currentSaveFolderName = folderName;
        }

        /// <summary>
        /// Returns path to game saves folder
        /// </summary>
        public static string SavesFolderPath 
        {
            get
            {
                return Path.Combine(GameSettings.BaseGamePath, SavesFolderName);
            }
        }

        /// <summary>
        /// Returns folder path to current save folder
        /// </summary>
        public static string CurrentSaveFolderPath 
        { 
            get 
            {
                if (string.IsNullOrEmpty(_currentSaveFolderName))
                { 
                    return string.Empty;
                }

                return Path.Combine(SavesFolderPath, _currentSaveFolderName);
            } 
        }

        /// <summary>
        /// Returns user data path from current save
        /// </summary>
        public static string CurrentUserGameDataPath
        {
            get
            {
                if (string.IsNullOrEmpty(_currentSaveFolderName))
                {
                    return string.Empty;
                }

                return Path.Combine(CurrentSaveFolderPath, UserGameDataFileName);
            }
        }
    }
}
