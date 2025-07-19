using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages.Staff.ManageTickets
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public TicketDto Ticket { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            Ticket = await client.GetFromJsonAsync<TicketDto>($"api/staff/tickets/{id}");
            if (Ticket == null) return RedirectToPage("List");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/");
            var response = await client.DeleteAsync($"api/staff/tickets/{id}");
            return RedirectToPage("List");
        }
    }
}