using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages.User
{
    public class DiscountCodesModel : PageModel
    {
        public List<DiscountCodeDto> AvailableCodes { get; set; } = new List<DiscountCodeDto>();
        public List<UserDiscountCodeDto> MyCodes { get; set; } = new List<UserDiscountCodeDto>();
        public int CurrentPoints { get; set; }

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

                        // Lấy điểm hiện tại
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
                                }
                            }
                        }

                        // Lấy mã giảm giá có sẵn
                        var availableResponse = await client.GetAsync("api/user/discount-codes/available");
                        if (availableResponse.IsSuccessStatusCode)
                        {
                            var availableJson = await availableResponse.Content.ReadAsStringAsync();
                            if (!string.IsNullOrEmpty(availableJson))
                            {
                                AvailableCodes = JsonSerializer.Deserialize<List<DiscountCodeDto>>(availableJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<DiscountCodeDto>();
                            }
                        }

                        // Lấy mã giảm giá đã đổi
                        var myCodesResponse = await client.GetAsync("api/user/discount-codes/my-codes");
                        if (myCodesResponse.IsSuccessStatusCode)
                        {
                            var myCodesJson = await myCodesResponse.Content.ReadAsStringAsync();
                            if (!string.IsNullOrEmpty(myCodesJson))
                            {
                                MyCodes = JsonSerializer.Deserialize<List<UserDiscountCodeDto>>(myCodesJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<UserDiscountCodeDto>();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải thông tin mã giảm giá.";
            }
        }
    }
} 