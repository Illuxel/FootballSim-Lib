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
        internal static string ODBPath { get; } = "C:\\Users\\illay\\Desktop\\FootbalLife-Lib-main\\DatabaseLayer\\Database\\FootbalLifeDB.db";


        public static string getOriginDataBasePath()
        {
            return ODBPath;
        }

        public static void SetConnectionString(string dataBasePath)
        {
            ConnectionString = "Data Source=" + dataBasePath + "\\FootbalLifeDB.db";
        }
    }
}