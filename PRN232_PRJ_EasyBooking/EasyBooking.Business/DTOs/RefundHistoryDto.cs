using System;

namespace EasyBooking.Business.DTOs
{
    public class RefundHistoryDto
    {
        public int RefundHistoryId { get; set; }
        public int TicketId { get; set; }
        public decimal RefundAmount { get; set; }
        public DateTime RefundTime { get; set; }
        public string? Reason { get; set; }
        public int UserId { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        
        // Thông tin từ Ticket
        public string? MovieTitle { get; set; }
        public string? CinemaName { get; set; }
        public string? RoomName { get; set; }
        public string? SeatNumber { get; set; }
        public DateTime? ShowtimeStart { get; set; }
        public string? UserName { get; set; }
    }
} 