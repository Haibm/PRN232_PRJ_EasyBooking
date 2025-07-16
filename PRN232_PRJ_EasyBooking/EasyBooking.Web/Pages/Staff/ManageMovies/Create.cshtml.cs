using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages.Staff.ManageMovies
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public MovieDto Movie { get; set; }
        public bool Success { get; set; }
        public List<string> AllGenres { get; set; } = new();
        [BindProperty]
        public List<string> SelectedGenres { get; set; } = new();
        public class GenreDto
        {
            public int GenreId { get; set; }
            public string Name { get; set; }
        }
        public async Task OnGetAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            var genres = await client.GetFromJsonAsync<List<GenreDto>>("api/staff/genres");
            AllGenres = genres != null ? genres.Select(g => g.Name).ToList() : new List<string>();
            Success = false;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            Movie.Genres = SelectedGenres;
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            var response = await client.PostAsJsonAsync("api/staff/movies", Movie);
            Success = response.IsSuccessStatusCode;
            if (Success)
            {
                ModelState.Clear();
                Movie = new MovieDto();
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, errorMsg);
            }
            // Reload genres for redisplay
            var genres = await client.GetFromJsonAsync<List<GenreDto>>("api/staff/genres");
            AllGenres = genres != null ? genres.Select(g => g.Name).ToList() : new List<string>();
            return Page();
        }
    }
} 