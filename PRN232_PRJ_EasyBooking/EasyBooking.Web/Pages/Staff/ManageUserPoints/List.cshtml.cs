using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages.Staff.ManageUserPoints
{
    public class ListModel : PageModel
    {
        public List<UserPointsDto> UserPoints { get; set; } = new List<UserPointsDto>();
        public int TotalUsers { get; set; }
        public int TotalEarnedPoints { get; set; }
        public int TotalUsedPoints { get; set; }
        public int TotalCurrentPoints { get; set; }

        [BindProperty]
        public string SearchTerm { get; set; } = "";
        [BindProperty]
        public int MinPoints { get; set; } = 0;
        [BindProperty]
        public int MaxPoints { get; set; } = 0;

        [BindProperty]
        public int UserId { get; set; }
        [BindProperty]
        public int Points { get; set; }
        [BindProperty]
        public string Description { get; set; } = "";

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

                        // Lấy danh sách user points
                        var response = await client.GetAsync("api/staff/user-points");
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            if (!string.IsNullOrEmpty(json))
                            {
                                UserPoints = JsonSerializer.Deserialize<List<UserPointsDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<UserPointsDto>();
                            }
                        }

                        // Tính toán thống kê
                        CalculateStatistics();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải danh sách điểm thưởng.";
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Dữ liệu không hợp lệ.";
                    return RedirectToPage();
                }

                var token = Request.Cookies["jwtToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new System.Uri("https://localhost:7087/");
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                        // Chuẩn bị dữ liệu thêm điểm
                        var addPointsData = new
                        {
                            UserId = UserId,
                            Points = Points,
                            Description = Description
                        };

                        var json = JsonSerializer.Serialize(addPointsData);
                        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                        
                        var response = await client.PostAsync("api/staff/user-points/add-points", content);
                        
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Success"] = $"Đã thêm {Points} điểm cho user thành công!";
                        }
                        else
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            TempData["Error"] = "Lỗi: " + errorContent;
                        }
                    }
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi thêm điểm.";
                return RedirectToPage();
            }
        }

        private void CalculateStatistics()
        {
            TotalUsers = UserPoints.Count;
            TotalEarnedPoints = UserPoints.Sum(up => up.TotalEarnedPoints);
            TotalUsedPoints = UserPoints.Sum(up => up.TotalUsedPoints);
            TotalCurrentPoints = UserPoints.Sum(up => up.CurrentPoints);
        }
    }
} 