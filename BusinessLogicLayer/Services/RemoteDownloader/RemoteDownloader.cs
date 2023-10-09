using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class RemoteDownloader
    {
        private string _url = "http://54.160.153.24:5000/";
        
        public async Task DatabaseDownload()
        {
            const string fileName = "FootbalLifeDB.db";
            await download(fileName);
        }

        public async Task DbLayerDownload()
        {
            const string fileName = "DatabaseLayer.dll";
            await download(fileName);
        }

        public async Task BlLayerDownload()
        {
            const string fileName = "BusinessLogicLayer.dll";
            await download(fileName);
        }

        private async Task download(string fileName)
        {
            string url = defineUrl(fileName);

            string directoryPath = definePath(fileName);
            string filePath = Path.Combine(directoryPath, fileName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            using (HttpClient client = new HttpClient())
            {
                //byte[] fileBytes = await client.GetByteArrayAsync(fileUrl);
                HttpResponseMessage response = await client.GetAsync(url);

                // Перевірка статус коду відповіді
                if (response.IsSuccessStatusCode)
                {
                    // Отримання вмісту файлу
                    Stream fileStream = await response.Content.ReadAsStreamAsync();

                    // Збереження вмісту у файлі
                    using (FileStream outputFileStream = File.Create(filePath))
                    {
                        await fileStream.CopyToAsync(outputFileStream);
                    }
                }
            }

        }

        private string defineUrl(string fileName)
        {
            switch (fileName)
            {
                case "BusinessLogicLayer.dll":
                    return _url + "businessLogicLayer";
                case "DatabaseLayer.dll":
                    return _url + "databaseLayer";
                case "FootbalLifeDB.db":
                    return _url + "database";
                default:
                    return null;
            }
        }

        private string definePath(string fileName)
        {
            switch (fileName)
            {
                case "BusinessLogicLayer.dll":
                    return Path.Combine(Directory.GetCurrentDirectory(), "BusinessLogicLayer");
                case "DatabaseLayer.dll":
                    return Path.Combine(Directory.GetCurrentDirectory(), "DatabaseLayer");
                case "FootbalLifeDB.db":
                    return Path.Combine(Directory.GetCurrentDirectory(), "Database");
                default:
                    return null;
            }
        }
    }
}
