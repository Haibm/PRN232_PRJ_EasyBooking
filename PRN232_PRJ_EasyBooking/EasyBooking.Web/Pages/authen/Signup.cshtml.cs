using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyBooking.Web.Pages.Authen
{
    public class SignupModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string FullName { get; set; }
        [BindProperty]
        public string Email { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(Email))
            {
                ErrorMessage = "All fields are required.";
                return Page();
            }

            var userDto = new { Username = Username, PasswordHash = Password, FullName = FullName, Email = Email, Role = "User", IsActive = true };
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("https://localhost:7087/");
                var content = new StringContent(JsonSerializer.Serialize(userDto), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/users", content);

                if (response.IsSuccessStatusCode)
                {
                    SuccessMessage = "Registration successful! You can now log in.";
                    return RedirectToPage("/Index");
                }
                else
                {
                    ErrorMessage = "Registration failed. Please try again.";
                }

                return Page();
            }
        }
    }
}
