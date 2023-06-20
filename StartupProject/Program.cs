
using BusinessLogicLayer.Scenario;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var startDate = new System.DateTime(2023, 08, 12);
            var gTour = new GenerateAllMatchesByTour(startDate);
            gTour.Generate();
        }
    }
}