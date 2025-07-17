using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using EasyBooking.Business.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EasyBooking.Web.Pages
{
    public class IndexModel : PageModel
    {
        public List<MovieDto> Movies { get; set; } = new();
        public string? CurrentStatus { get; set; }
        public List<string> StatusList { get; set; } = new() { "All", "Sắp Chiếu", "Đang Chiếu" };

        private string MapStatusToDb(string? status)
        {
            return status switch
            {
                "Sắp Chiếu" => "Upcoming",
                "Đang Chiếu" => "NowShowing",
                _ => ""
            };
        }

        public async Task OnGetAsync(string? status)
        {
            CurrentStatus = status;
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            var movies = await client.GetFromJsonAsync<List<MovieDto>>("/api/user/movies");
            if (movies != null)
            {
                if (!string.IsNullOrEmpty(status) && status != "All")
                {
                    var dbStatus = MapStatusToDb(status);
                    Movies = movies.Where(m => m.Status == dbStatus).ToList();
                }
                else
                {
                    Movies = movies;
                }
            }
        }
    }
}
