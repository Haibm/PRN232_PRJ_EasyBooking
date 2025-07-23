using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EasyBooking.Web.Pages.authen
{
    public class OrderHistoryModel : PageModel
    {
        public List<OrderHistoryDto> OrderHistories { get; set; } = new();
        public async Task OnGetAsync()
        {
            var token = Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(token)) return;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://localhost:7087/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var res = await client.GetAsync("api/user/orders/history");
                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    OrderHistories = JsonSerializer.Deserialize<List<OrderHistoryDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
        }
    }
}
