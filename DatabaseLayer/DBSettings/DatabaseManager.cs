using System.IO;

namespace DatabaseLayer.DBSettings
{
    public static class DatabaseManager
    {
        private static string _connectionString;
        internal static string ConnectionString { 
            get
            {   
                if(string.IsNullOrEmpty(_connectionString))
                {
                    return _defaultConnectionString;
                }
                return _connectionString;
            } 
            set 
            {
                _connectionString = value; 
            } 
        }
        internal static string PathToSave { get; set; }
        internal static string SavePathInfo;
        internal static string OriginalDbFileName = "FootbalLifeDB.db";
        internal static string UserDataFileName = "UserData.json";
        internal static string OriginalDbFilePath = Path.Combine(".", "Database", OriginalDbFileName);
        private static string _defaultConnectionString = "Data Source=" + OriginalDbFilePath;


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