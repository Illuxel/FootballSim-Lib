using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class DatabaseDownloader
    {
        private string _fileUrl = "http://54.160.153.24:5000/download";
        public async Task Download()
        {
            await download(Directory.GetCurrentDirectory());
        }

        public async Task Download(string path)
        {
            download(path);
        }

        private async Task download(string path)
        {
            var directoryPath = Path.Combine(path, "Database");
            string filePath = Path.Combine(directoryPath, "FootbalLifeDB.db");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            using (HttpClient client = new HttpClient())
            {
                //byte[] fileBytes = await client.GetByteArrayAsync(fileUrl);
                HttpResponseMessage response = await client.GetAsync(_fileUrl);

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
    }
}
