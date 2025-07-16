using Microsoft.AspNetCore.Mvc.RazorPages;
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
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/"); // Sửa lại nếu API chạy port khác
            var rooms = await client.GetFromJsonAsync<List<RoomDto>>("api/staff/rooms");
            // Lấy danh sách rạp để map tên
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