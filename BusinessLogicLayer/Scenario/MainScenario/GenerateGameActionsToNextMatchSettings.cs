namespace BusinessLogicLayer.Scenario
{
    public class GenerateGameActionsToNextMatchSettings
    {
        public static string ApplicationBasePath { get; set; }
        public GenerateGameActionsToNextMatchSettings(string appBasePath)
        {
            ApplicationBasePath = appBasePath;
        }
    }
}
