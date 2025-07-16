using Microsoft.AspNetCore.Mvc.RazorPages;
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
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/"); // Sửa lại nếu API chạy port khác
            Cinemas = await client.GetFromJsonAsync<List<CinemaDto>>("api/staff/cinemas");
        }
    }
} 