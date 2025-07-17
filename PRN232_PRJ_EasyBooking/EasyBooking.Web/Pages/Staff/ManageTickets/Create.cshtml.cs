using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages.Staff.ManageTickets
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public TicketDto Ticket { get; set; }
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
            client.BaseAddress = new System.Uri("https://localhost:7087/"); // Sửa lại nếu API chạy port khác
            var response = await client.PostAsJsonAsync("api/staff/tickets", Ticket);
            Success = response.IsSuccessStatusCode;
            if (Success)
            {
                ModelState.Clear();
                Ticket = new TicketDto();
            }
            return Page();
        }
    }
}