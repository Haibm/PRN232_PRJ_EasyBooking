using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;
using System.Collections.Generic;

namespace EasyBooking.Web.Pages.Staff.ManageShowtimes
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public ShowtimeDto Showtime { get; set; }
        public bool Success { get; set; }
        public List<MovieDto> AllMovies { get; set; } = new();
        public List<RoomDto> AllRooms { get; set; } = new();

        public async Task OnGetAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            AllMovies = await client.GetFromJsonAsync<List<MovieDto>>("api/staff/movies");
            AllRooms = await client.GetFromJsonAsync<List<RoomDto>>("api/staff/rooms");
            Success = false;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                using var client = new HttpClient();
                client.BaseAddress = new System.Uri("https://localhost:7087/");
                AllMovies = await client.GetFromJsonAsync<List<MovieDto>>("api/staff/movies");
                AllRooms = await client.GetFromJsonAsync<List<RoomDto>>("api/staff/rooms");
                return Page();
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://localhost:7087/");
                var response = await client.PostAsJsonAsync("api/staff/showtimes", Showtime);
                Success = response.IsSuccessStatusCode;
                if (Success)
                {
                    ModelState.Clear();
                    Showtime = new ShowtimeDto();
                }
                AllMovies = await client.GetFromJsonAsync<List<MovieDto>>("api/staff/movies");
                AllRooms = await client.GetFromJsonAsync<List<RoomDto>>("api/staff/rooms");
                return Page();
            }
        }
    }
} 