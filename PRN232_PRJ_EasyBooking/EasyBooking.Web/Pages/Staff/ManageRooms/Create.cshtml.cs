using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;
using System.Collections.Generic;

namespace EasyBooking.Web.Pages.Staff.ManageRooms
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public RoomDto Room { get; set; }
        public bool Success { get; set; }
        public List<CinemaDto> AllCinemas { get; set; } = new();

        public async Task OnGetAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            AllCinemas = await client.GetFromJsonAsync<List<CinemaDto>>("api/staff/cinemas");
            Success = false;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                using var client = new HttpClient();
                client.BaseAddress = new System.Uri("https://localhost:7087/");
                AllCinemas = await client.GetFromJsonAsync<List<CinemaDto>>("api/staff/cinemas");
                return Page();
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://localhost:7087/");
                var response = await client.PostAsJsonAsync("api/staff/rooms", Room);
                Success = response.IsSuccessStatusCode;
                if (Success)
                {
                    ModelState.Clear();
                    Room = new RoomDto();
                }
                AllCinemas = await client.GetFromJsonAsync<List<CinemaDto>>("api/staff/cinemas");
                return Page();
            }
        }
    }
} 