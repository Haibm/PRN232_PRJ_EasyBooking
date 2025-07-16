using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;
using System.Linq;

namespace EasyBooking.Web.Pages.Staff.ManageMovies
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public MovieDto Movie { get; set; }
        [BindProperty]
        public List<string> SelectedGenres { get; set; } = new();
        public List<string> AllGenres { get; set; } = new();
        public bool Success { get; set; }

        public class GenreDto
        {
            public int GenreId { get; set; }
            public string Name { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/"); // Sửa lại nếu API chạy port khác
            Movie = await client.GetFromJsonAsync<MovieDto>($"api/staff/movies/{id}");
            if (Movie == null) return RedirectToPage("/Staff/ManageMovies/List");
            SelectedGenres = Movie.Genres ?? new List<string>();
            // Lấy tất cả thể loại từ API genres
            var genres = await client.GetFromJsonAsync<List<GenreDto>>("api/staff/genres");
            AllGenres = genres?.Select(g => g.Name).ToList() ?? new List<string>();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return Page();
            Movie.MovieId = id;

            Movie.Genres = SelectedGenres;
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            var response = await client.PutAsJsonAsync($"api/staff/movies/{id}", Movie);
            Success = response.IsSuccessStatusCode;
            if (Success)
            {
                // Reload lại dữ liệu mới nhất
                Movie = await client.GetFromJsonAsync<MovieDto>($"api/staff/movies/{id}");
                SelectedGenres = Movie.Genres ?? new List<string>();
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, errorMsg);
            }
            // Lấy lại danh sách thể loại
            var genres = await client.GetFromJsonAsync<List<GenreDto>>("api/staff/genres");
            AllGenres = genres?.Select(g => g.Name).ToList() ?? new List<string>();
            return Page();
        }
    }
} 