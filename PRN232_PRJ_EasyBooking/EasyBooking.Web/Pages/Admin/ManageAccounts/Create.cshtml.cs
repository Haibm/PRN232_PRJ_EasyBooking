using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages.Admin.ManageAccounts
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public UserDto User { get; set; }
        public bool Success { get; set; }

        public void OnGet()
        {
            Success = false;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            var response = await client.PostAsJsonAsync("api/admin/accounts", User);
            Success = response.IsSuccessStatusCode;
            if (Success)
            {
                ModelState.Clear();
                User = new UserDto();
            }
            return Page();
        }
    }
}
