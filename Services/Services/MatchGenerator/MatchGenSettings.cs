using System.Reflection;

namespace FootBalLife.Services.MatchGenerator
{
    public static class MatchGenSettings
    {
        public static string EventsFilePath = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/Events.json";
    }
}
