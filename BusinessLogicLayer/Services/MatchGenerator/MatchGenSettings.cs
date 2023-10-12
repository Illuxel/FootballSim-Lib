using System.IO;

namespace BusinessLogicLayer.Services
{
    //rename, => static 
    public static class MatchGenSettings
    {
        public static string BasePath { get; set; }
        internal static string EventsPath { get; private set; }

       /* private MatchGenSettings(string basePath)
        {
            EventsPath = Path.Combine(basePath, "Events.json");
            BasePath = basePath;
        }*/

        public static MatchGenSettings GetInstance(string basePath)
        {
            return new MatchGenSettings(BasePath);
        }
    }

}
