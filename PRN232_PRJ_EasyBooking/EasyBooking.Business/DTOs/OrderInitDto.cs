namespace EasyBooking.Business.DTOs
{
    public class OrderInitDto
    {
        public string MovieTitle { get; set; }
        public string ShowtimeStr { get; set; }
        public string CinemaName { get; set; }
        public string RoomName { get; set; }
        public List<string> Seats { get; set; } = new List<string>();
        public decimal TotalPrice { get; set; }
    }
} 