using BusinessLogicLayer.Scenario;
using System.IO;

namespace BusinessLogicLayer.Services
{
    public static class MatchGenSettings
    {
        public static string EventsFilePath = Path.Combine(GenerateGameActionsToNextMatchSettings.ApplicationBasePath, "Events.json");
    }
}
