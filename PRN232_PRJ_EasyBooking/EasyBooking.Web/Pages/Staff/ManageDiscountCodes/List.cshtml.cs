using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using EasyBooking.Business.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EasyBooking.Web.Pages.Staff.ManageDiscountCodes
{
    public class ListModel : PageModel
    {
        public List<DiscountCodeDto> DiscountCodes { get; set; } = new List<DiscountCodeDto>();
        public int TotalCodes { get; set; }
        public int ActiveCodes { get; set; }
        public int TotalUsage { get; set; }
        public int ExpiringCodes { get; set; }

        [BindProperty]
        public DiscountCodeDto DiscountCode { get; set; } = new DiscountCodeDto();

        private string GetUsernameFromToken()
        {
            try
            {
                var token = Request.Cookies["jwtToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadJwtToken(token);
                    
                    var usernameClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "username" || c.Type == "sub");
                    if (usernameClaim != null)
                    {
                        return usernameClaim.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing JWT token: {ex.Message}");
            }
            
            return "Staff"; // Fallback
        }

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

                        // Lấy danh sách mã giảm giá
                        var response = await client.GetAsync("api/staff/discount-codes");
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            if (!string.IsNullOrEmpty(json))
                            {
                                DiscountCodes = JsonSerializer.Deserialize<List<DiscountCodeDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<DiscountCodeDto>();
                            }
                        }

                        // Tính toán thống kê
                        CalculateStatistics();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải danh sách mã giảm giá.";
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Debug: Log tất cả form data
                foreach (var key in Request.Form.Keys)
                {
                    Console.WriteLine($"Form[{key}]: {Request.Form[key]}");
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    TempData["Error"] = "Dữ liệu không hợp lệ: " + string.Join(", ", errors);
                    return RedirectToPage();
                }

                var token = Request.Cookies["jwtToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new System.Uri("https://localhost:7087/");
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                        // Chuẩn bị dữ liệu
                        var username = GetUsernameFromToken();
                        var discountCodeDto = new DiscountCodeDto
                        {
                            DiscountCodeId = DiscountCode.DiscountCodeId,
                            Code = Request.Form["Code"].ToString(),
                            Name = Request.Form["Name"].ToString(),
                            Description = Request.Form["Description"].ToString(),
                            RequiredPoints = int.Parse(Request.Form["RequiredPoints"].ToString()),
                            MaxUsage = int.Parse(Request.Form["MaxUsage"].ToString()),
                            StartDate = DateTime.Parse(Request.Form["StartDate"].ToString()),
                            EndDate = DateTime.Parse(Request.Form["EndDate"].ToString()),
                            IsActive = true, // Mặc định kích hoạt khi tạo mới
                            CreateBy = username,
                            UpdateBy = username,
                            DeleteBy = "",
                            CreateAt = DateTime.Now,
                            IsDelete = false
                        };

                        // Xử lý loại giảm giá
                        var discountType = Request.Form["discountType"].ToString();
                        var discountValue = decimal.Parse(Request.Form["DiscountValue"].ToString());
                        
                        if (discountType == "percentage")
                        {
                            discountCodeDto.DiscountPercentage = discountValue;
                            discountCodeDto.DiscountAmount = 0;
                        }
                        else
                        {
                            discountCodeDto.DiscountAmount = discountValue;
                            discountCodeDto.DiscountPercentage = 0;
                        }

                        // Debug: Log dữ liệu
                        Console.WriteLine($"DiscountCodeId: {discountCodeDto.DiscountCodeId}");
                        Console.WriteLine($"Code: {discountCodeDto.Code}");
                        Console.WriteLine($"Name: {discountCodeDto.Name}");
                        Console.WriteLine($"DiscountType: {discountType}");
                        Console.WriteLine($"DiscountValue: {discountValue}");
                        Console.WriteLine($"StartDate: {discountCodeDto.StartDate}");
                        Console.WriteLine($"EndDate: {discountCodeDto.EndDate}");

                        // Gửi request tạo hoặc sửa mã giảm giá
                        var json = JsonSerializer.Serialize(discountCodeDto);
                        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                        
                        HttpResponseMessage response;
                        if (discountCodeDto.DiscountCodeId > 0)
                        {
                            // Sửa mã giảm giá
                            response = await client.PutAsync($"api/staff/discount-codes/{discountCodeDto.DiscountCodeId}", content);
                        }
                        else
                        {
                            // Thêm mã giảm giá mới
                            response = await client.PostAsync("api/staff/discount-codes", content);
                        }
                        
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Success"] = discountCodeDto.DiscountCodeId > 0 ? "Sửa mã giảm giá thành công!" : "Thêm mã giảm giá thành công!";
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
                TempData["Error"] = "Có lỗi xảy ra khi thêm/sửa mã giảm giá.";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
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

                        // Lấy thông tin mã giảm giá trước khi xóa
                        var getResponse = await client.GetAsync($"api/staff/discount-codes/{id}");
                        
                        if (getResponse.IsSuccessStatusCode)
                        {
                            var json = await getResponse.Content.ReadAsStringAsync();
                            var discountCode = JsonSerializer.Deserialize<DiscountCodeDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            
                            if (discountCode != null)
                            {
                                // Soft delete thay vì hard delete
                                discountCode.IsDelete = true;
                                discountCode.DeleteBy = GetUsernameFromToken();
                                discountCode.DeleteAt = DateTime.Now;
                                
                                var updateJson = JsonSerializer.Serialize(discountCode);
                                var content = new StringContent(updateJson, System.Text.Encoding.UTF8, "application/json");
                                
                                var updateResponse = await client.PutAsync($"api/staff/discount-codes/{id}", content);
                                
                                if (updateResponse.IsSuccessStatusCode)
                                {
                                    TempData["Success"] = "Xóa mã giảm giá thành công!";
                                }
                                else
                                {
                                    TempData["Error"] = "Có lỗi xảy ra khi xóa mã giảm giá.";
                                }
                            }
                        }
                    }
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa mã giảm giá.";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostEditAsync(int id)
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

                        var response = await client.GetAsync($"api/staff/discount-codes/{id}");
                        
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var discountCode = JsonSerializer.Deserialize<DiscountCodeDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            
                            if (discountCode != null)
                            {
                                // Lưu dữ liệu vào TempData để hiển thị trong modal
                                TempData["EditDiscountCode"] = JsonSerializer.Serialize(discountCode);
                                TempData["ShowEditModal"] = "true";
                            }
                        }
                    }
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải thông tin mã giảm giá.";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostToggleActiveAsync(int id)
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

                        // Lấy thông tin mã giảm giá hiện tại
                        var getResponse = await client.GetAsync($"api/staff/discount-codes/{id}");
                        
                        if (getResponse.IsSuccessStatusCode)
                        {
                            var json = await getResponse.Content.ReadAsStringAsync();
                            var discountCode = JsonSerializer.Deserialize<DiscountCodeDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            
                            if (discountCode != null)
                            {
                                // Đảo ngược trạng thái
                                discountCode.IsActive = !discountCode.IsActive;
                                discountCode.UpdateBy = GetUsernameFromToken();
                                discountCode.UpdateAt = DateTime.Now;
                                
                                // Gửi request cập nhật
                                var updateJson = JsonSerializer.Serialize(discountCode);
                                var content = new StringContent(updateJson, System.Text.Encoding.UTF8, "application/json");
                                
                                var updateResponse = await client.PutAsync($"api/staff/discount-codes/{id}", content);
                                
                                if (updateResponse.IsSuccessStatusCode)
                                {
                                    TempData["Success"] = discountCode.IsActive ? "Kích hoạt mã giảm giá thành công!" : "Vô hiệu hóa mã giảm giá thành công!";
                                }
                                else
                                {
                                    TempData["Error"] = "Có lỗi xảy ra khi cập nhật trạng thái mã giảm giá.";
                                }
                            }
                        }
                    }
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi cập nhật trạng thái mã giảm giá.";
                return RedirectToPage();
            }
        }

        private void CalculateStatistics()
        {
            TotalCodes = DiscountCodes.Count;
            ActiveCodes = DiscountCodes.Count(c => c.IsActive);
            TotalUsage = DiscountCodes.Sum(c => c.CurrentUsage);
            
            var now = DateTime.Now;
            var thirtyDaysFromNow = now.AddDays(30);
            ExpiringCodes = DiscountCodes.Count(c => c.EndDate <= thirtyDaysFromNow && c.EndDate > now);
        }
    }
} 