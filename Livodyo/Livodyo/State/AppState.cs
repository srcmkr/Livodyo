using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LiteDB;
using Livodyo.Models;
using Newtonsoft.Json;

namespace Livodyo.State
{
    public class AppState
    {
        public List<AudioBookModel> AudioBooks { get; set; }
        public List<AuthorModel> Authors { get; set; }
        public List<TagModel> Tags { get; set; }

        public List<AudioBookModel> DownloadedAudioBooks { get; set; }

        private HttpClient Client { get; set; }

        // uncomment for productive using
        public string ApiEndpoint = "http://hsh2brain.privacy.ltd:8080";

        // uncomment use for android debugging
        //public string ApiEndpoint = "https://10.0.2.2:6001";
            
        // uncomment for UWP debugging
        //public string ApiEndpoint = "https://localhost:6001";

        public AppState()
        {
            var handler = new HttpClientHandler() 
            { 
                ServerCertificateCustomValidationCallback = delegate { return true; },
                
            };
            Client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(5)
            };
        }

        public async Task<T> GetAsync<T>(string url)
        {
            try
            {
                var content = await Client.GetAsync(url);
                return !content.IsSuccessStatusCode
                    ? default
                    : JsonConvert.DeserializeObject<T>(await content.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex}");
                return default;
            }
        }

        public void Save()
        {
            var dbFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "livodio.db");
            using (var db = new LiteDatabase(dbFile))
            {
                var col = db.GetCollection<AudioBookModel>("audiobooks");
                col.DeleteAll();
                col.InsertBulk(DownloadedAudioBooks);
            }
        }

        public async Task SynchronizeAsync()
        {
            // preparing links to api
            var audioBookEndpoint = $"{ApiEndpoint}/audiobooks/";
            var authorsEndpoint = $"{ApiEndpoint}/authors/";
            var tagsEndpoint = $"{ApiEndpoint}/tags/";

            // get/download every list of entitytype from api
            AudioBooks = await GetAsync<List<AudioBookModel>>(audioBookEndpoint);
            Authors = await GetAsync<List<AuthorModel>>(authorsEndpoint);
            Tags = await GetAsync<List<TagModel>>(tagsEndpoint);

            var dbFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "livodio.db");
            using (var db = new LiteDatabase(dbFile))
            {
                var col = db.GetCollection<AudioBookModel>("audiobooks");
                DownloadedAudioBooks = col.FindAll().ToList();
            }
        }
    }
}