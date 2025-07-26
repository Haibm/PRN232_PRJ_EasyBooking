using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Linq;

namespace EasyBooking.Web.Pages.authen
{
    public class OrderHistoryModel : PageModel
    {
        public List<OrderHistoryDto> OrderHistories { get; set; } = new();
        public List<TicketDetailDto> Tickets { get; set; } = new();
        public List<RefundHistoryDto> RefundedTickets { get; set; } = new();
        [BindProperty]
        public int RefundTicketId { get; set; }
        [BindProperty]
        public string RefundReason { get; set; }
        [BindProperty]
        public int ShowTicketsOrderId { get; set; }

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

        public async Task<IActionResult> OnPostShowTicketsAsync(int showTicketsOrderId)
        {
            var token = Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(token)) return Page();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://localhost:7087/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                
                // Lấy vé đang hoạt động (chưa hoàn)
                var res = await client.GetAsync($"api/user/tickets/by-order/{showTicketsOrderId}");
                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    Tickets = JsonSerializer.Deserialize<List<TicketDetailDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                
                // Lấy vé đã hoàn từ RefundHistory
                var refundRes = await client.GetAsync($"api/user/tickets/refunds/by-order/{showTicketsOrderId}");
                if (refundRes.IsSuccessStatusCode)
                {
                    var refundJson = await refundRes.Content.ReadAsStringAsync();
                    RefundedTickets = JsonSerializer.Deserialize<List<RefundHistoryDto>>(refundJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            ShowTicketsOrderId = showTicketsOrderId;
            await OnGetAsync(); // reload order history
            return Page();
        }

        public async Task<IActionResult> OnPostRefundAsync()
        {
            var token = Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(token)) return RedirectToPage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://localhost:7087/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var body = JsonSerializer.Serialize(new { RefundReason });
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var res = await client.PostAsync($"api/user/tickets/{RefundTicketId}/refund", content);
                if (res.IsSuccessStatusCode)
                {
                    TempData["RefundSuccess"] = true;
                }
                else
                {
                    var errorResponse = await res.Content.ReadAsStringAsync();
                    try
                    {
                        var errorObj = JsonSerializer.Deserialize<JsonElement>(errorResponse);
                        if (errorObj.TryGetProperty("message", out var messageElement))
                        {
                            TempData["RefundError"] = messageElement.GetString();
                        }
                        else
                        {
                            TempData["RefundError"] = "Không thể hoàn vé. Vui lòng thử lại sau.";
                        }
                    }
                    catch
                    {
                        TempData["RefundError"] = "Không thể hoàn vé. Vui lòng thử lại sau.";
                    }
                }
            }
            return RedirectToPage();
        }
    }
}
