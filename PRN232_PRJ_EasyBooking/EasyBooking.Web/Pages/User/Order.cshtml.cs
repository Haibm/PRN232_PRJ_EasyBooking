using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using EasyBooking.Business.DTOs;
using System.Text.Json;
using System.Net.Http;
using System;
using System.Threading.Tasks;

namespace EasyBooking.Web.Pages.User
{
    public class OrderModel : PageModel
    {
        [BindProperty]
        public OrderInitDto Order { get; set; }
        public string UserId { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7087/");
                var token = Request.Cookies["jwtToken"] ?? Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Add("Authorization", token);

                // Lấy orderData từ TempData (hoặc FE gửi lên qua body nếu cần)
                var orderDataJson = TempData["OrderInitDto"] as string;
                if (string.IsNullOrEmpty(orderDataJson))
                    return RedirectToPage("/User/OrderShowtimes");

                var content = new StringContent(orderDataJson, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/user/orders/init", content);

                if (!response.IsSuccessStatusCode)
                    return RedirectToPage("/User/OrderShowtimes");

                var responseBody = await response.Content.ReadAsStringAsync();
                Order = System.Text.Json.JsonSerializer.Deserialize<OrderInitDto>(responseBody, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/User/OrderSuccess");
        }

        public async Task<IActionResult> OnPostInitAsync([FromBody] OrderInitDto order)
        {
            Console.WriteLine("===== OnPostInitAsync called =====");
            Console.WriteLine("OrderData: " + System.Text.Json.JsonSerializer.Serialize(order));
            if (order == null || order.Seats == null || order.Seats.Count == 0)
                return new JsonResult(new { error = "Invalid order" }) { StatusCode = 400 };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7087/");
                var token = Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Add("Authorization", token);

                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(order), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/user/orders/payment", content);

                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var jsonDoc = System.Text.Json.JsonDocument.Parse(responseBody);
                    var paymentUrl = jsonDoc.RootElement.GetProperty("paymentUrl").GetString();
                    return new JsonResult(new { success = true, paymentUrl });
                }
                else
                {
                    return new JsonResult(new { error = responseBody }) { StatusCode = (int)response.StatusCode };
                }
            }
        }
    }
}
