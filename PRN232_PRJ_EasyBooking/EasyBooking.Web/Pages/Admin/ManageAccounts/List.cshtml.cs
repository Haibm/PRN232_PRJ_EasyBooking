using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Web.Pages.Admin.ManageAccounts
{
    public class ListModel : PageModel
    {
        public List<UserDto> Users { get; set; }

        public async Task OnGetAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/"); 
            Users = await client.GetFromJsonAsync<List<UserDto>>("api/admin/accounts");
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7087/"); // Đúng port API của bạn
            var response = await client.DeleteAsync($"api/users/{id}");
            if (response.IsSuccessStatusCode)
            {
                Users = await client.GetFromJsonAsync<List<UserDto>>("api/admin/accounts");
            }
            else
            {
                // Có thể thêm thông báo lỗi nếu cần
            }
            return Page();
        }
    }
}
