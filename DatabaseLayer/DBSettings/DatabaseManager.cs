using System.IO;

namespace DatabaseLayer.DBSettings
{
    public static class DatabaseManager
    {
        internal static string ConnectionString { get; set; }
        internal static string PathToSave { get; set; }
        internal static string SavePathInfo;
        internal static string OriginalDbFileName = "FootbalLifeDB.db";
        internal static string UserDataFileName = "UserData.json";
        internal static string OriginalDbFilePath = Path.Combine(".", "Database", OriginalDbFileName);


        public static string getOriginDataBasePath()
        {
            return OriginalDbFileName;
        }

        public static void SetConnectionString(string dataBasePath)
        {
            ConnectionString = "Data Source=" + Path.Combine(dataBasePath, OriginalDbFileName);
        }
    }
}