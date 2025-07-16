using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyBooking.Web.Pages.Authen
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Username and password are required.";
                return Page();
            }

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("https://localhost:7087/");

                var loginDto = new { Username = Username, PasswordHash = Password };
                var content = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/users/login", content);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;
                    var role = root.GetProperty("role").GetString();
                    var username = root.GetProperty("username").GetString();
                    var password = root.GetProperty("passwordHash").GetString();
                    var userId = root.GetProperty("userId").GetInt32();


                    HttpContext.Session.SetString("Username", Username);
                    HttpContext.Session.SetString("Password", Password);
                    HttpContext.Session.SetString("Role", role);
                    HttpContext.Session.SetInt32("UserId", userId);

                    if (role == "Admin")
                        return RedirectToPage("/Admin/Dashboard");
                    else if (role == "Staff")
                        return RedirectToPage("/Staff/ManageCinemas/List");
                    else
                        return RedirectToPage("/Index");
                }
                else
                {
                    ErrorMessage = "Invalid username or password.";
                    return Page();
                }
            }
        }
    }
}
