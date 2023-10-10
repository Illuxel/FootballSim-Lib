using System.IO;

namespace DatabaseLayer.Settings
{
    public static class DatabaseSettings
    {
        internal const string DBFolderName = "Database";
        internal const string DBFileName = "FootbalLifeDB.db";

        /// <summary>
        /// Returns base database path. Not from current save
        /// </summary>
        public static string BaseDBPath
        {
            get
            {
                return Path.Combine(GameSettings.BaseGamePath, DBFolderName, DBFileName);
            }
        }

        public static string DBFolderPath
        {
            get
            {
                if (string.IsNullOrEmpty(SavesSettings.CurrentSaveFolderPath))
                {
                    return Path.Combine(GameSettings.BaseGamePath, DBFolderName);
                }

                return Path.Combine(SavesSettings.CurrentSaveFolderPath, DBFolderName);
            }
        }
        public static string DBFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(SavesSettings.CurrentSaveFolderPath))
                {
                    return BaseDBPath;
                }

                return Path.Combine(SavesSettings.CurrentSaveFolderPath, DBFolderName, DBFileName);
            }
        }

        internal static string ConnectionString
        {
            get
            {
                return $"Data Source={DBFilePath}";
            }
        }
    }
}