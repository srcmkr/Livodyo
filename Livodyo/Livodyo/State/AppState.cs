using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Livodyo.Models;
using Newtonsoft.Json;

namespace Livodyo.State
{
    public class AppState
    {
        public List<AudioBookModel> AudioBooks { get; set; }
        public List<AuthorModel> Authors { get; set; }
        public List<TagModel> Tags { get; set; }
        private HttpClient Client { get; set; }

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
                Console.WriteLine("API Error", ex);
                return default;
            }

        }

        public async Task SynchronizeAsync()
        {
            const string apiEndpoint = "https://localhost:6001";
            var audioBookEndpoint = $"{apiEndpoint}/audiobooks/";
            var authorsEndpoint = $"{apiEndpoint}/authors/";
            var tagsEndpoint = $"{apiEndpoint}/tags/";

            AudioBooks = await GetAsync<List<AudioBookModel>>(audioBookEndpoint);
            Authors = await GetAsync<List<AuthorModel>>(authorsEndpoint);
            Tags = await GetAsync<List<TagModel>>(tagsEndpoint);
        }
    }
}

// kbo2cHNdMeI