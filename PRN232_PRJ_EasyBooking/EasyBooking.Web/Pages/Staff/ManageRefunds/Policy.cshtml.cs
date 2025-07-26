using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;
using System;

namespace EasyBooking.Web.Pages.Staff.ManageRefunds
{
    public class PolicyModel : PageModel
    {
        public List<RefundPolicyDto> Policies { get; set; } = new();
        public RefundPolicyDto ActivePolicy { get; set; }
        
        [BindProperty]
        public int RefundPolicyId { get; set; }
        
        [BindProperty]
        public string PolicyName { get; set; }
        
        [BindProperty]
        public decimal RefundPercentage { get; set; }
        
        [BindProperty]
        public string Description { get; set; }
        
        [BindProperty]
        public bool IsActive { get; set; }

        public async Task OnGetAsync()
        {
            var token = Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(token)) return;
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://localhost:7087/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                
                // Lấy danh sách chính sách
                var policiesRes = await client.GetAsync("api/staff/refund-policies");
                if (policiesRes.IsSuccessStatusCode)
                {
                    var json = await policiesRes.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(json))
                    {
                        Policies = JsonSerializer.Deserialize<List<RefundPolicyDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                }
                
                // Lấy chính sách đang hoạt động
                var activePolicyRes = await client.GetAsync("api/staff/refund-policies/active");
                if (activePolicyRes.IsSuccessStatusCode)
                {
                    var activeJson = await activePolicyRes.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(activeJson))
                    {
                        try
                        {
                            // Thử parse như object trước để kiểm tra có data field không
                            var responseObj = JsonSerializer.Deserialize<JsonElement>(activeJson);
                            if (responseObj.TryGetProperty("data", out var dataProp) && dataProp.ValueKind == JsonValueKind.Null)
                            {
                                // Không có active policy
                                ActivePolicy = null;
                            }
                            else
                            {
                                // Có active policy, parse như RefundPolicyDto
                                ActivePolicy = JsonSerializer.Deserialize<RefundPolicyDto>(activeJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            }
                        }
                        catch
                        {
                            // Fallback: thử parse trực tiếp như RefundPolicyDto
                            ActivePolicy = JsonSerializer.Deserialize<RefundPolicyDto>(activeJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        }
                    }
                }
            }
        }

        public async Task<IActionResult> OnPostSavePolicyAsync()
        {
            var token = Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(token)) return RedirectToPage();
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://localhost:7087/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                
                var policyData = new
                {
                    RefundPolicyId = RefundPolicyId,
                    PolicyName = PolicyName,
                    RefundPercentage = RefundPercentage,
                    Description = Description,
                    IsActive = IsActive,
                    CreateAt = DateTime.Now,
                    CreateBy = "Staff"
                };
                
                var json = JsonSerializer.Serialize(policyData);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                
                HttpResponseMessage response;
                if (RefundPolicyId == 0)
                {
                    // Thêm mới
                    response = await client.PostAsync("api/staff/refund-policies", content);
                }
                else
                {
                    // Cập nhật
                    response = await client.PutAsync($"api/staff/refund-policies/{RefundPolicyId}", content);
                }
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = RefundPolicyId == 0 ? "Thêm chính sách thành công!" : "Cập nhật chính sách thành công!";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                    
                    try
                    {
                        var errorObj = JsonSerializer.Deserialize<JsonElement>(errorContent);
                        if (errorObj.TryGetProperty("error", out var errorProp))
                        {
                            TempData["Error"] = $"Lỗi: {errorProp.GetString()}";
                        }
                        else if (errorObj.TryGetProperty("message", out var messageProp))
                        {
                            TempData["Error"] = $"Lỗi: {messageProp.GetString()}";
                        }
                        else
                        {
                            TempData["Error"] = $"Lỗi API: {response.StatusCode} - {errorContent}";
                        }
                    }
                    catch
                    {
                        TempData["Error"] = $"Lỗi API: {response.StatusCode} - {errorContent}";
                    }
                }
            }
            
            return RedirectToPage();
        }
    }
} 