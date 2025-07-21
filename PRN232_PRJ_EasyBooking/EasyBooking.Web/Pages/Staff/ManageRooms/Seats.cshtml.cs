using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EasyBooking.Web.Pages.Staff.ManageRooms
{
    public class SeatsModel : PageModel
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; } = "";

        public void OnGet(int roomId)
        {
            RoomId = roomId;
            // RoomName có thể lấy từ DB nếu cần
        }
    }
} 