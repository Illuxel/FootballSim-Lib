using BusinessLogicLayer.Scenario;
using BusinessLogicLayer.Services;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var gTour1 = new GenerateAllMatchesByTour(new System.DateTime(2023, 08, 12));
            gTour1.Generate();
            var gTour2 = new GenerateAllMatchesByTour(new System.DateTime(2023, 08, 19));
            gTour2.Generate();
            var gTour3 = new GenerateAllMatchesByTour(new System.DateTime(2023, 08, 26));
            gTour3.Generate();
            /*var train = new PlayerSkillsTrainer();
            train.TrainPlayers("B5551778D1672E4E544F32BFFAD52BA6",DatabaseLayer.Enums.TrainingMode.AdvancedForLastGameBench);*/
        }
    }
}
