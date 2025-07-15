using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages.Staff.ManageCinemas
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public CinemaDto Cinema { get; set; }
        public bool Success { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            Cinema = await client.GetFromJsonAsync<CinemaDto>($"api/staff/cinemas/{id}");
            if (Cinema == null) return RedirectToPage("/Staff/ManageCinemas/List");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return Page();
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            var response = await client.PutAsJsonAsync($"api/staff/cinemas/{id}", Cinema);
            Success = response.IsSuccessStatusCode;
            if (Success)
            {
                // Thành công, chuyển về List
                return RedirectToPage("/Staff/ManageCinemas/List");
            }
            // Nếu lỗi thực sự, giữ lại form
            return Page();
        }
    }
} 