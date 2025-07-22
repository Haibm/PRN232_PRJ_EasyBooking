using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Linq;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages.User
{
    public class OrderShowtimesModel : PageModel
    {
        public List<OrderShowtimeDTO.Cinema> Cinemas { get; set; } = new();
        public List<OrderShowtimeDTO.Room> Rooms { get; set; } = new();
        public List<DateTime> Days { get; set; } = new();
        public List<OrderShowtimeDTO.Movie> Movies { get; set; } = new();
        public int SelectedCinemaId { get; set; }
        public DateTime SelectedDay { get; set; }
        public int? UserId { get; set; }

        public async Task OnGetAsync(int? cinemaId, string? day)
        {
            UserId = HttpContext.Session.GetInt32("UserId");
            // Lấy danh sách rạp
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7087/");
                var res = await client.GetAsync("api/staff/cinemas");
                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    Cinemas = JsonSerializer.Deserialize<List<OrderShowtimeDTO.Cinema>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });    
                }
            }
            // Lấy danh sách ngày (7 ngày tới)
            Days = new List<DateTime>();
            for (int i = 0; i < 7; i++) Days.Add(DateTime.Today.AddDays(i));
            // Xác định rạp/ngày được chọn
            if (Cinemas.Count == 0)
            {
                SelectedCinemaId = 0;
                SelectedDay = DateTime.Today;
                Movies = new List<OrderShowtimeDTO.Movie>();
                return;
            }
            var validCinema = Cinemas.Find(c => c.CinemaId == cinemaId);
            if (validCinema == null)
            {
                SelectedCinemaId = Cinemas[0].CinemaId;
            }
            else
            {
                SelectedCinemaId = validCinema.CinemaId;
            }
            SelectedDay = day != null ? DateTime.Parse(day) : DateTime.Today;

            // Lấy tất cả rooms
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7087/");
                var res = await client.GetAsync("api/staff/rooms");
                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    Rooms = JsonSerializer.Deserialize<List<OrderShowtimeDTO.Room>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }

            // Lấy tất cả showtimes
            List<OrderShowtimeDTO.Showtime> allShowtimes = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7087/");
                var res = await client.GetAsync("api/staff/showtimes");
                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    allShowtimes = JsonSerializer.Deserialize<List<OrderShowtimeDTO.Showtime>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            // Join Room với Showtime để lấy CinemaId
            var showtimesWithCinema = allShowtimes.Select(s => new
            {
                Showtime = s,
                CinemaId = Rooms.FirstOrDefault(r => r.RoomId == s.RoomId)?.CinemaId ?? 0
            }).ToList();
            // Lọc showtimes theo cinemaId và ngày
            var filteredShowtimes = showtimesWithCinema
                .Where(x => x.CinemaId == SelectedCinemaId && x.Showtime.StartTime.Date == SelectedDay.Date)
                .Select(x => x.Showtime)
                .ToList();

            // Lấy tất cả movies (chỉ lấy các trường cơ bản, KHÔNG lấy Showtimes từ API)
            List<OrderShowtimeDTO.Movie> allMovies = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7087/");
                var res = await client.GetAsync("api/staff/movies");
                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    var tempMovies = JsonSerializer.Deserialize<List<TempMovie>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    allMovies = tempMovies.Select(m => new OrderShowtimeDTO.Movie
                    {
                        MovieId = m.MovieId,
                        Title = m.Title,
                        Description = m.Description,
                        Duration = m.Duration,
                        PosterUrl = m.PosterUrl,
                        Status = m.Status,
                        Genres = m.Genres,
                        Showtimes = new List<OrderShowtimeDTO.Showtime>() // sẽ gán sau
                    }).ToList();
                }
            }
            // Join movies với showtimes
            Movies = allMovies
                .Where(m => filteredShowtimes.Any(s => s.MovieId == m.MovieId))
                .Select(m => {
                    m.Showtimes = filteredShowtimes.Where(s => s.MovieId == m.MovieId).ToList();
                    return m;
                })
                .ToList();
        }
    }

    // DTO tạm cho deserialize
    class TempMovie
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string PosterUrl { get; set; }
        public string Status { get; set; }
        public List<string> Genres { get; set; }
        // Không còn VipPercent ở đây
    }
}
