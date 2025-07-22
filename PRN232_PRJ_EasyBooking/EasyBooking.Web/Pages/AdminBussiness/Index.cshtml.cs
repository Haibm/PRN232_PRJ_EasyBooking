using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace EasyBooking.Web.Pages.AdminBussiness
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public int TotalMovies { get; set; }
        public int TotalUsersBooked { get; set; }
        public decimal TotalPaid { get; set; }
        public string TopSpender { get; set; }
        public string TopMovie { get; set; }

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
  
            var response = await client.GetAsync("https://localhost:7087/api/admin/dashboard/stats");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                TotalMovies = root.GetProperty("totalMovies").GetInt32();
                TotalUsersBooked = root.GetProperty("totalUsersBooked").GetInt32();
                TotalPaid = root.GetProperty("totalPaid").GetDecimal();
                TopSpender = root.TryGetProperty("topSpender", out var ts) && ts.ValueKind != JsonValueKind.Null
                    ? ts.GetProperty("fullName").GetString()
                    : "N/A";
                TopMovie = root.TryGetProperty("topMovie", out var tm) && tm.ValueKind != JsonValueKind.Null
                    ? tm.GetProperty("title").GetString()
                    : "N/A";
            }
        }
    }
}
