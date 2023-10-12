using BusinessLogicLayer.Services;
using DatabaseLayer.Services;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var save = LoadGameManager.GetInstance().Load("test");
            var matchGenSettings = new MatchGenSettings("D:\\FootbalLife-Lib\\StartupProject\\bin\\Debug\\net7.0\\");

            var matchGen = new MatchGenerator("d5389dcd-9b56-453f-a019-0a43c0c022a2",matchGenSettings);
            matchGen.StartGenerating();
        }
    }
}