using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;
using System.Linq;

namespace EasyBooking.Web.Pages.Staff.ManageRooms
{
    public class ListModel : PageModel
    {
        public List<RoomViewModel> Rooms { get; set; }

        public class RoomViewModel : RoomDto
        {
            public string CinemaName { get; set; }
        }

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            await client.DeleteAsync($"api/staff/rooms/{id}");

            // Reload lại dữ liệu sau khi xoá
            await LoadDataAsync();
            return Page();
        }

        private async Task LoadDataAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            var rooms = await client.GetFromJsonAsync<List<RoomDto>>("api/staff/rooms");
            var cinemas = await client.GetFromJsonAsync<List<CinemaDto>>("api/staff/cinemas");
            Rooms = rooms.Select(r => new RoomViewModel
            {
                RoomId = r.RoomId,
                Name = r.Name,
                Capacity = r.Capacity,
                Location = r.Location,
                CinemaId = r.CinemaId,
                CinemaName = cinemas.FirstOrDefault(c => c.CinemaId == r.CinemaId)?.Name ?? ""
            }).ToList();
        }
    }
}
