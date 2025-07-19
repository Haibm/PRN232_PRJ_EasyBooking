using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;
using System.Linq;

namespace EasyBooking.Web.Pages.Staff.ManageShowtimes
{
    public class ListModel : PageModel
    {
        public List<ShowtimeViewModel> Showtimes { get; set; }

        public class ShowtimeViewModel : ShowtimeDto
        {
            public string MovieTitle { get; set; }
            public string RoomName { get; set; }
        }

        public async Task OnGetAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            var showtimes = await client.GetFromJsonAsync<List<ShowtimeDto>>("api/staff/showtimes");
            var movies = await client.GetFromJsonAsync<List<MovieDto>>("api/staff/movies");
            var rooms = await client.GetFromJsonAsync<List<RoomDto>>("api/staff/rooms");
            Showtimes = showtimes.Select(s => new ShowtimeViewModel
            {
                ShowtimeId = s.ShowtimeId,
                MovieId = s.MovieId,
                RoomId = s.RoomId,
                StartTime = s.StartTime,
                Price = s.Price,
                MovieTitle = movies.FirstOrDefault(m => m.MovieId == s.MovieId)?.Title ?? "",
                RoomName = rooms.FirstOrDefault(r => r.RoomId == s.RoomId)?.Name ?? ""
            }).ToList();
        }
    }
} 