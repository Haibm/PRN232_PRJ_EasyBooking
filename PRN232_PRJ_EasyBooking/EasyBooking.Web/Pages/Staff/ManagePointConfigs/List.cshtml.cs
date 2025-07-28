using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using EasyBooking.Business.DTOs;
using EasyBooking.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EasyBooking.Web.Pages.Staff.ManagePointConfigs
{
    public class ListModel : PageModel
    {
        public List<PointConfigDto> PointConfigs { get; set; } = new List<PointConfigDto>();
        public int TotalConfigs { get; set; }
        public int ActiveConfigs { get; set; }
        public int DefaultConfigs { get; set; }
        public int CustomConfigs { get; set; }

        [BindProperty]
        public PointConfigDto PointConfig { get; set; } = new PointConfigDto();

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
                Console.WriteLine($"JWT Token: {(string.IsNullOrEmpty(token) ? "NULL" : "EXISTS")}");
                
                // Test trực tiếp database trước
                Console.WriteLine("Testing direct database access...");
                try
                {
                    using (var context = new CinemaBookingDbContext())
                    {
                        var rawConfigs = await context.PointConfigs
                            .Where(pc => pc.IsDelete != true)
                            .ToListAsync();
                        Console.WriteLine($"Direct DB query returned {rawConfigs.Count} configs");
                        
                        if (rawConfigs.Count > 0)
                        {
                            PointConfigs = rawConfigs.Select(c => new PointConfigDto
                            {
                                PointConfigId = c.PointConfigId,
                                ConfigName = c.ConfigName,
                                Description = c.Description,
                                AmountPerPoint = c.AmountPerPoint,
                                MinAmountToEarn = c.MinAmountToEarn,
                                IsActive = c.IsActive,
                                CreateAt = c.CreateAt,
                                CreateBy = c.CreateBy,
                                UpdateAt = c.UpdateAt,
                                UpdateBy = c.UpdateBy,
                                DeleteAt = c.DeleteAt,
                                DeleteBy = c.DeleteBy,
                                IsDelete = c.IsDelete
                            }).ToList();
                            Console.WriteLine($"Mapped {PointConfigs.Count} configs from direct DB query");
                            
                            // Tính toán thống kê
                            CalculateStatistics();
                            return; // Sử dụng data từ database
                        }
                    }
                }
                catch (Exception dbEx)
                {
                    Console.WriteLine($"Database access error: {dbEx.Message}");
                }
                
                // Nếu database không có data, thử API
                if (!string.IsNullOrEmpty(token))
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new System.Uri("https://localhost:7087/");
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                        // Lấy danh sách cấu hình điểm
                        var response = await client.GetAsync("api/staff/point-configs");
                        Console.WriteLine($"API Response Status: {response.StatusCode}");
                        
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"API Response JSON: {json}");
                            
                            if (!string.IsNullOrEmpty(json))
                            {
                                PointConfigs = JsonSerializer.Deserialize<List<PointConfigDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<PointConfigDto>();
                                Console.WriteLine($"Deserialized PointConfigs Count: {PointConfigs.Count}");
                            }
                            else
                            {
                                Console.WriteLine("API returned empty JSON");
                            }
                        }
                        else
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                            
                            // Thử endpoint test không cần authentication
                            Console.WriteLine("Trying test endpoint...");
                            var testResponse = await client.GetAsync("api/staff/point-configs/test");
                            Console.WriteLine($"Test API Response Status: {testResponse.StatusCode}");
                            
                            if (testResponse.IsSuccessStatusCode)
                            {
                                var testJson = await testResponse.Content.ReadAsStringAsync();
                                Console.WriteLine($"Test API Response JSON: {testJson}");
                            }
                        }

                        // Tính toán thống kê
                        CalculateStatistics();
                        Console.WriteLine($"Statistics: Total={TotalConfigs}, Active={ActiveConfigs}, Default={DefaultConfigs}, Custom={CustomConfigs}");
                    }
                }
                else
                {
                    Console.WriteLine("No JWT token found, using database data only");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in OnGetAsync: {ex.Message}");
                Console.WriteLine($"Exception StackTrace: {ex.StackTrace}");
                TempData["Error"] = "Có lỗi xảy ra khi tải danh sách cấu hình điểm.";
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
                        var pointConfigDto = new PointConfigDto
                        {
                            PointConfigId = PointConfig.PointConfigId,
                            ConfigName = Request.Form["ConfigName"].ToString(),
                            Description = Request.Form["Description"].ToString(),
                            AmountPerPoint = int.Parse(Request.Form["AmountPerPoint"].ToString()),
                            MinAmountToEarn = decimal.Parse(Request.Form["MinAmountToEarn"].ToString()),
                            IsActive = Request.Form.ContainsKey("IsActive") && Request.Form["IsActive"].ToString() == "true",
                            CreateBy = username,
                            UpdateBy = username,
                            CreateAt = DateTime.Now,
                            UpdateAt = DateTime.Now,
                            DeleteBy = "",
                            IsDelete = false
                        };

                        // Debug: Log dữ liệu
                        Console.WriteLine($"PointConfigId: {pointConfigDto.PointConfigId}");
                        Console.WriteLine($"ConfigName: {pointConfigDto.ConfigName}");
                        Console.WriteLine($"AmountPerPoint: {pointConfigDto.AmountPerPoint}");
                        Console.WriteLine($"MinAmountToEarn: {pointConfigDto.MinAmountToEarn}");
                        Console.WriteLine($"IsActive: {pointConfigDto.IsActive}");

                        // Gửi request tạo hoặc sửa cấu hình điểm
                        var json = JsonSerializer.Serialize(pointConfigDto);
                        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                        
                        HttpResponseMessage response;
                        if (pointConfigDto.PointConfigId > 0)
                        {
                            // Sửa cấu hình điểm
                            response = await client.PutAsync($"api/staff/point-configs/{pointConfigDto.PointConfigId}", content);
                        }
                        else
                        {
                            // Thêm cấu hình điểm mới
                            response = await client.PostAsync("api/staff/point-configs", content);
                        }
                        
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Success"] = pointConfigDto.PointConfigId > 0 ? "Sửa cấu hình điểm thành công!" : "Thêm cấu hình điểm thành công!";
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
                TempData["Error"] = "Có lỗi xảy ra khi thêm/sửa cấu hình điểm.";
                return RedirectToPage();
            }
        }

        private void CalculateStatistics()
        {
            Console.WriteLine($"CalculateStatistics called with {PointConfigs.Count} configs");
            
            TotalConfigs = PointConfigs.Count;
            ActiveConfigs = PointConfigs.Count(c => c.IsActive);
            DefaultConfigs = PointConfigs.Count(c => c.ConfigName.ToLower().Contains("mặc định"));
            CustomConfigs = TotalConfigs - DefaultConfigs;
            
            Console.WriteLine($"Calculated: Total={TotalConfigs}, Active={ActiveConfigs}, Default={DefaultConfigs}, Custom={CustomConfigs}");
            
            // Debug: Log từng config
            foreach (var config in PointConfigs)
            {
                Console.WriteLine($"Config: ID={config.PointConfigId}, Name={config.ConfigName}, IsActive={config.IsActive}");
            }
        }
    }
} 