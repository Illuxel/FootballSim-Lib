using BusinessLogicLayer.Scenario;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var gen = new GenerateGameActionsToNextMatch(new System.DateTime(2023,08,12), "678065FDDB06C590A0D0F9EDC2B5196F");
            gen.SimulateActions();
        }
    }
}