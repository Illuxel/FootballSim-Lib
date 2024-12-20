﻿using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RemoteAccessFiles
{
    public class RemoteDownloader
    {
        private string _url = "http://54.160.153.24:5000/";

        public async Task DatabaseDownload(string path = null)
        {
            await download("Database", true, path);
        }

        public async Task DbLayerDownload(string path = null)
        {
            await download("DatabaseLayer", false, path);
        }

        public async Task BlLayerDownload(string path = null)
        {
            await download("BusinessLogicLayer", false, path);
        }

        private async Task download(string endpoint, bool isDb, string path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Directory.GetCurrentDirectory();
            }
            string fullUrl = _url + endpoint;

            var fileName = string.Empty;
            if (isDb)
            {
                fileName = "FootbalLifeDB.db";
            }
            else
            {
                fileName = endpoint + ".dll";
            }
            string filePath = Path.Combine(path, fileName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (HttpClient client = new HttpClient())
            {
                //byte[] fileBytes = await client.GetByteArrayAsync(fileUrl);
                HttpResponseMessage response = await client.GetAsync(fullUrl);

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