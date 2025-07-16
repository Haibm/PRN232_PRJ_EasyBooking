using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;
using System.Collections.Generic;

namespace EasyBooking.Web.Pages.Staff.ManageShowtimes
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public ShowtimeDto Showtime { get; set; }
        public bool Success { get; set; }
        public List<MovieDto> AllMovies { get; set; } = new();
        public List<RoomDto> AllRooms { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            Showtime = await client.GetFromJsonAsync<ShowtimeDto>($"api/staff/showtimes/{id}");
            if (Showtime == null) return RedirectToPage("/Staff/ManageShowtimes/List");
            AllMovies = await client.GetFromJsonAsync<List<MovieDto>>("api/staff/movies");
            AllRooms = await client.GetFromJsonAsync<List<RoomDto>>("api/staff/rooms");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
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
                Showtime.ShowtimeId = id;
                var response = await client.PutAsJsonAsync($"api/staff/showtimes/{id}", Showtime);
                Success = response.IsSuccessStatusCode;
                if (Success)
                {
                    // Thành công, chuyển về List
                    return RedirectToPage("/Staff/ManageShowtimes/List");
                }
                AllMovies = await client.GetFromJsonAsync<List<MovieDto>>("api/staff/movies");
                AllRooms = await client.GetFromJsonAsync<List<RoomDto>>("api/staff/rooms");
                return Page();
            }
        }
    }
} 