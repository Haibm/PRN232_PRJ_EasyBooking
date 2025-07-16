using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages.Staff.ManageGenres
{
    public class ListModel : PageModel
    {
        public List<GenreDto> Genres { get; set; }

        public async Task OnGetAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/"); // Sửa lại nếu API chạy port khác
            Genres = await client.GetFromJsonAsync<List<GenreDto>>("api/staff/genres");
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            await client.DeleteAsync($"api/staff/genres/{id}");
            // Sau khi xóa, reload lại danh sách
            Genres = await client.GetFromJsonAsync<List<GenreDto>>("api/staff/genres");
            return Page();
        }
    }
} 