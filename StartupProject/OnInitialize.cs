using System.IO;
using System.Threading.Tasks;

using DatabaseLayer.Settings;
using BusinessLogicLayer.Services;

namespace StartupProject
{
    internal class OnInitialize
    {
        public static async Task Main(string[] args)
        {
            GameSettings.BaseGamePath = Directory.GetCurrentDirectory();

            var dbDownloader = new RemoteDownloader();
            await dbDownloader.DatabaseDownload();
            Program.Main(args);
        }
    }
}
