using System.IO;
using System.Reflection;

namespace BusinessLogicLayer.Services
{
    public static class MatchGenSettings
    {
        public static string EventsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Events.json");
    }
}
