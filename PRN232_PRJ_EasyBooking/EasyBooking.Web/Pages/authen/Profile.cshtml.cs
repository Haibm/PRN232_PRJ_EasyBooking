using EasyBooking.Business.DTOs;
using EasyBooking.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyBooking.Web.Pages.Authen
{
    public class ProfileModel : PageModel
    {
        [BindProperty]
        public UserDto User { get; set; }
        public bool Success { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Success = false;

            var username = HttpContext.Session.GetString("Username");
            var password = HttpContext.Session.GetString("Password");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || userId == null)
                return RedirectToPage("/Authen/Login");

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("https://localhost:7087/");
                var userResponse = await client.GetAsync($"api/users/{userId}");
                if (!userResponse.IsSuccessStatusCode)
                    return RedirectToPage("/Authen/Login");

                var userJson = await userResponse.Content.ReadAsStringAsync();
                User = JsonSerializer.Deserialize<UserDto>(userJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Success = false;

            var username = HttpContext.Session.GetString("Username");
            var password = HttpContext.Session.GetString("Password");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || userId == null)
                return RedirectToPage("/Authen/Login");

            User.UserId = userId.Value;

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("https://localhost:7087/");
                var userJson = JsonSerializer.Serialize(User);
                var content = new StringContent(userJson, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"api/users/{userId}", content);
                if (response.IsSuccessStatusCode)
                {
                    Success = true;
                    return RedirectToPage();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Cập nhật thông tin thất bại.");
                    return Page();
                }
            }
        }
        
    }
}
