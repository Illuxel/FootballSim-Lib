using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Security.Cryptography;
using System.Linq;

namespace DatabaseLayer.DBSettings
{
    public static class DatabaseManager
    {
        internal static string ConnectionString { get; set; }
       

        internal static string _pathToSave { get; set; }
        internal static string _savePathInfo;
        internal static string _originalDbFileName = "FootbalLifeDB.db";
        internal static string _userDataFileName = "UserData.json";
        internal static string _originalDbFilePath { get; } = Path.Combine(".", "Database", _originalDbFileName);


        public static string getOriginDataBasePath()
        {
            return _originalDbFileName;
        }

        public static void SetConnectionString(string dataBasePath)
        {
            ConnectionString = "Data Source=" + Path.Combine(dataBasePath, _originalDbFileName);
        }
    }
}