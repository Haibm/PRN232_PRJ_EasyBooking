using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages.Admin.ManageAccounts
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public UserDto User { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            User = await client.GetFromJsonAsync<UserDto>($"api/admin/accounts/{id}");
            if (User == null) return RedirectToPage("/Admin/ManageAccounts/List");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return Page();
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            
            var isActiveValue = Request.Form["User.IsActive"].ToString();
            User.IsActive = isActiveValue == "true" || isActiveValue == "True";
            var response = await client.PutAsJsonAsync($"api/admin/accounts/{id}", User);
            Success = response.IsSuccessStatusCode;
            if (Success)
            {
                return RedirectToPage("/Admin/ManageAccounts/List");
            }
            ErrorMessage = await response.Content.ReadAsStringAsync();
            return Page();
        }
    }
}
