using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages.Staff.ManageGenres
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public GenreDto Genre { get; set; }
        public bool Success { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            Genre = await client.GetFromJsonAsync<GenreDto>($"api/staff/genres/{id}");
            if (Genre == null) return RedirectToPage("/Staff/ManageGenres/List");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return Page();
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            var response = await client.PutAsJsonAsync($"api/staff/genres/{id}", Genre);
            Success = response.IsSuccessStatusCode;
            if (Success)
            {
                // Thành công, chuyển về List
                return RedirectToPage("/Staff/ManageGenres/List");
            }
            // Nếu lỗi thực sự, giữ lại form
            return Page();
        }
    }
} 