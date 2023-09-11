using System.IO;

namespace BusinessLogicLayer.Services
{
    public static class MatchGenSettings
    {
        public static string EventsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Events.json");
    }
}
