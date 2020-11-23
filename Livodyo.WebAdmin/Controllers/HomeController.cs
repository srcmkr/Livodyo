using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Livodyo.Models;
using Livodyo.WebAdmin.Lib;
using Livodyo.WebAdmin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Livodyo.WebAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _http;

        public HomeController()
        {
            _http = new HttpClient();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var audiobooks = await _http.GetFromJsonAsync<List<AudioBookModel>>($"{Helpers.GetEnvironmentVariable("API")}/audiobooks");
            var tags = await _http.GetFromJsonAsync<List<TagModel>>($"{Helpers.GetEnvironmentVariable("API")}/tags");
            var authors = await _http.GetFromJsonAsync<List<AuthorModel>>($"{Helpers.GetEnvironmentVariable("API")}/authors");
            
            return View(new MovielistViewModel{ AudioBooks = audiobooks, Tags = tags, Authors = authors });
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag(MovielistViewModel vm)
        {
            var debugReturn = await _http.PostAsJsonAsync($"{Helpers.GetEnvironmentVariable("API")}/tags", vm.NewTag);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor(MovielistViewModel vm)
        {
            var debugReturn = await _http.PostAsJsonAsync($"{Helpers.GetEnvironmentVariable("API")}/authors", vm.NewAuthor);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAudioBook(MovielistViewModel vm)
        {
            var debugReturn = await _http.PostAsJsonAsync($"{Helpers.GetEnvironmentVariable("API")}/audiobooks", vm.NewAudioBook);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAudioBook(Guid audiobookId)
        {
            var debugReturn = await _http.DeleteAsync($"{Helpers.GetEnvironmentVariable("API")}/audiobooks/{audiobookId}");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteTag(Guid tagId)
        {
            var debugReturn = await _http.DeleteAsync($"{Helpers.GetEnvironmentVariable("API")}/tags/{tagId}");
            return RedirectToAction(nameof(Index));
        }
    }
}
