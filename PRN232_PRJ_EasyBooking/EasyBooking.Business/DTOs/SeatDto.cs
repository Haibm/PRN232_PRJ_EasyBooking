namespace EasyBooking.Business.DTOs
{
    public class SeatDto
    {
        public int SeatId { get; set; }
        public int RoomId { get; set; }
        public string? RowLetter { get; set; }
        public int SeatNumber { get; set; }
        public string? SeatType { get; set; }
    }
} 