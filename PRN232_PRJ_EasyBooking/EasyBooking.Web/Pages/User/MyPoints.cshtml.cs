using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages.User
{
    public class MyPointsModel : PageModel
    {
        public int CurrentPoints { get; set; }
        public int TotalEarnedPoints { get; set; }
        public int TotalUsedPoints { get; set; }
        public List<PointTransactionDto> PointTransactions { get; set; } = new List<PointTransactionDto>();
        public PointConfigDto PointConfig { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var token = Request.Cookies["jwtToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new System.Uri("https://localhost:7087/");
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                        // Lấy thông tin điểm
                        var pointsResponse = await client.GetAsync("api/user/user-points/my-points");
                        if (pointsResponse.IsSuccessStatusCode)
                        {
                            var pointsJson = await pointsResponse.Content.ReadAsStringAsync();
                            if (!string.IsNullOrEmpty(pointsJson))
                            {
                                var userPoints = JsonSerializer.Deserialize<UserPointsDto>(pointsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                                if (userPoints != null)
                                {
                                    CurrentPoints = userPoints.CurrentPoints;
                                    TotalEarnedPoints = userPoints.TotalEarnedPoints;
                                    TotalUsedPoints = userPoints.TotalUsedPoints;
                                }
                            }
                        }

                        // Lấy lịch sử giao dịch điểm
                        var transactionsResponse = await client.GetAsync("api/user/point-transactions/my-transactions");
                        if (transactionsResponse.IsSuccessStatusCode)
                        {
                            var transactionsJson = await transactionsResponse.Content.ReadAsStringAsync();
                            if (!string.IsNullOrEmpty(transactionsJson))
                            {
                                PointTransactions = JsonSerializer.Deserialize<List<PointTransactionDto>>(transactionsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<PointTransactionDto>();
                            }
                        }

                        // Lấy cấu hình điểm hiện tại
                        var configResponse = await client.GetAsync("api/staff/point-configs/active");
                        if (configResponse.IsSuccessStatusCode)
                        {
                            var configJson = await configResponse.Content.ReadAsStringAsync();
                            if (!string.IsNullOrEmpty(configJson))
                            {
                                PointConfig = JsonSerializer.Deserialize<PointConfigDto>(configJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải thông tin điểm thưởng.";
            }
        }
    }
} 