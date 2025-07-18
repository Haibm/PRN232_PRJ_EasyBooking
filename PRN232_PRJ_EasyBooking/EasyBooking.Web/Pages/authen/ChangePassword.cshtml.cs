using EasyBooking.Business.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyBooking.Web.Pages.authen
{
    public class ChangePasswordModel : PageModel
    {
        [BindProperty]
        public string OldPassword { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public string ConfirmPassword { get; set; }
        [BindProperty]
        public string VerificationCode { get; set; }

        public string SuccessMsg { get; set; }
        public string ErrorMsg { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostSendCodeAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                ErrorMsg = "Không tìm thấy thông tin người dùng.";
                return Page();
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://localhost:7087/");
                var response = await client.PostAsync($"api/users/{userId}/send-change-password-code", null);
                if (response.IsSuccessStatusCode)
                {
                    SuccessMsg = "Đã gửi mã xác nhận đến email của bạn.";
                }
                else
                {
                    ErrorMsg = "Không thể gửi mã xác nhận. Vui lòng thử lại.";
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                ErrorMsg = "Không tìm thấy thông tin người dùng.";
                return Page();
            }
            var dto = new ChangePasswordDto
            {
                UserId = userId.Value,
                OldPassword = OldPassword,
                NewPassword = NewPassword,
                ConfirmPassword = ConfirmPassword,
                VerificationCode = VerificationCode
            };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://localhost:7087/");
                var json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"api/users/{userId}/change-password", content);
                if (response.IsSuccessStatusCode)
                {
                    SuccessMsg = "Đổi mật khẩu thành công.";
                }
                else
                {
                    var resp = await response.Content.ReadAsStringAsync();
                    ErrorMsg = "Đổi mật khẩu thất bại. " + resp;
                }
            }
            return Page();
        }
    }
}
