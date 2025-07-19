using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;
using System.Collections.Generic;

namespace EasyBooking.Web.Pages.Staff.ManageRooms
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public RoomDto Room { get; set; }
        public bool Success { get; set; }
        public List<CinemaDto> AllCinemas { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            Room = await client.GetFromJsonAsync<RoomDto>($"api/staff/rooms/{id}");
            if (Room == null) return RedirectToPage("/Staff/ManageRooms/List");
            AllCinemas = await client.GetFromJsonAsync<List<CinemaDto>>("api/staff/cinemas");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
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
                Room.RoomId = id;
                var response = await client.PutAsJsonAsync($"api/staff/rooms/{id}", Room);
                Success = response.IsSuccessStatusCode;
                if (Success)
                {
                    // Thành công, chuyển về List
                    return RedirectToPage("/Staff/ManageRooms/List");
                }
                AllCinemas = await client.GetFromJsonAsync<List<CinemaDto>>("api/staff/cinemas");
                return Page();
            }
        }
    }
} 