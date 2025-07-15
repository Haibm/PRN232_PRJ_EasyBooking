using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages.Staff.ManageCinemas
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CinemaDto Cinema { get; set; }
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
            var response = await client.PostAsJsonAsync("api/staff/cinemas", Cinema);
            Success = response.IsSuccessStatusCode;
            if (Success)
            {
                ModelState.Clear();
                Cinema = new CinemaDto();
            }
            return Page();
        }
    }
} 