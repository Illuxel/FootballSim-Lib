using BusinessLogicLayer.Services;
using System.Threading.Tasks;

namespace StartupProject
{
    internal class OnInitialize
    {
        public static async Task Main(string[] args)
        {
            var dbDownloader = new RemoteDownloader();
            /*await dbDownloader.DatabaseDownload();*/
            Program.Main(args);
        }
    }
}
