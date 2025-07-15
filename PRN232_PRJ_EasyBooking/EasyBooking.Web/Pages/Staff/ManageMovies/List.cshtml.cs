using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages.Staff.ManageMovies
{
    public class ListModel : PageModel
    {
        public List<MovieDto> Movies { get; set; }

        public async Task OnGetAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/"); // Sửa lại nếu API chạy port khác
            Movies = await client.GetFromJsonAsync<List<MovieDto>>("api/staff/movies");
        }
    }
} 