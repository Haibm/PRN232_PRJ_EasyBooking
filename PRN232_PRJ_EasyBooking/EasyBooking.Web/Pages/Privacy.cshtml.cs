using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        public RefundPolicyDto ActiveRefundPolicy { get; set; }

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
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
                        
                        // Lấy chính sách hoàn vé đang hoạt động
                        var response = await client.GetAsync("api/staff/refund-policies/active");
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            if (!string.IsNullOrEmpty(json))
                            {
                                try
                                {
                                    // Thử parse như object trước để kiểm tra có data field không
                                    var responseObj = JsonSerializer.Deserialize<JsonElement>(json);
                                    if (responseObj.TryGetProperty("data", out var dataProp) && dataProp.ValueKind == JsonValueKind.Null)
                                    {
                                        // Không có active policy
                                        ActiveRefundPolicy = null;
                                    }
                                    else
                                    {
                                        // Có active policy, parse như RefundPolicyDto
                                        ActiveRefundPolicy = JsonSerializer.Deserialize<RefundPolicyDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                                    }
                                }
                                catch
                                {
                                    // Fallback: thử parse trực tiếp như RefundPolicyDto
                                    ActiveRefundPolicy = JsonSerializer.Deserialize<RefundPolicyDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading refund policy");
                // Nếu không load được, vẫn hiển thị trang với thông tin mặc định
            }
        }
    }
}
