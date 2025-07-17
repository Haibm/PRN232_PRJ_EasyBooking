using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages.Admin.ManageAccounts
{
    public class DeleteModel : PageModel
    {
        public UserDto User { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            User = await client.GetFromJsonAsync<UserDto>($"api/admin/accounts/{id}");
            if (User == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            var response = await client.DeleteAsync($"api/admin/accounts/{id}");
            return RedirectToPage("/Admin/ManageAccounts/List");
        }
    }
}
