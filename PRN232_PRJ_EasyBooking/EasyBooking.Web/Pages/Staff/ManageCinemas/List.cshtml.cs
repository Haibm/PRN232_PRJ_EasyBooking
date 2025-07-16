using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages.Staff.ManageCinemas
{
    public class ListModel : PageModel
    {
        public List<CinemaDto> Cinemas { get; set; }

        public async Task OnGetAsync()
        {
            await LoadCinemasAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            await client.DeleteAsync($"api/staff/cinemas/{id}");

            // Load lại danh sách sau khi xóa
            await LoadCinemasAsync();
            return Page();
        }

        private async Task LoadCinemasAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            Cinemas = await client.GetFromJsonAsync<List<CinemaDto>>("api/staff/cinemas");
        }
    }
}
