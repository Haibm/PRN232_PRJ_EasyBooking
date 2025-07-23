using EasyBooking.Business.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace EasyBooking.Web.Pages.Authen
{
    public class ProfileModel : PageModel
    {
        [BindProperty]
        public UserDto User { get; set; }

        public bool Success { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/authen/Login");

            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
                return RedirectToPage("/authen/Login");

            JwtSecurityToken jwt;
            try
            {
                jwt = handler.ReadJwtToken(token);
            }
            catch
            {
                return RedirectToPage("/authen/Login");
            }

            var userId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "userId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToPage("/authen/Login");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://localhost:7087/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var response = await client.GetAsync($"api/users/{userId}");
                if (!response.IsSuccessStatusCode)
                    return RedirectToPage("/authen/Login");

                var userJson = await response.Content.ReadAsStringAsync();
                User = JsonSerializer.Deserialize<UserDto>(userJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Success = false;
            var token = Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/authen/Login");

            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
                return RedirectToPage("/authen/Login");

            JwtSecurityToken jwt;
            try
            {
                jwt = handler.ReadJwtToken(token);
            }
            catch
            {
                return RedirectToPage("/authen/Login");
            }

            var userId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "userId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToPage("/authen/Login");

            User.UserId = int.Parse(userId);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://localhost:7087/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

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