using System.IO;
using System.Reflection;

namespace BusinessLogicLayer.Services.MatchGenerator
{
    public static class MatchGenSettings
    {
        public static string EventsFilePath = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/Events.json";
    }
}
