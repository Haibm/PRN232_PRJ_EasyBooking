using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EasyBooking.Web.Pages.User
{
    public class DetailModel : PageModel
    {
        public MovieDetailDto? MovieDetail { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using var client = new HttpClient();
            var apiUrl = $"https://localhost:7087/api/user/movies/{id}/detail";
            MovieDetail = await client.GetFromJsonAsync<MovieDetailDto>(apiUrl);
            if (MovieDetail == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
