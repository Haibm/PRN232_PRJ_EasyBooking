namespace EasyBooking.Business.DTOs
{
    public class SeatDto
    {
        public int SeatId { get; set; }
        public int RoomId { get; set; }
        public string? RowLetter { get; set; }
        public int SeatNumber { get; set; }
        public string? SeatType { get; set; }

        // Thêm các trường quản lý
        public bool? IsDelete { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string? DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
    }
} 