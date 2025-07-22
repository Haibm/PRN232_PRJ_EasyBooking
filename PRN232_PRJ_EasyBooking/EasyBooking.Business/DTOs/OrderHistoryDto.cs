using System;

namespace EasyBooking.Business.DTOs
{
    public class OrderHistoryDto
    {
        public int OrderHistoryId { get; set; }
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? OrderNote { get; set; }
        public string? OrderStatus { get; set; }
    }
} 