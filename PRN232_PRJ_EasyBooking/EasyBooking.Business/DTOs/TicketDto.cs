using System;
using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Business.DTOs
{
    public class TicketDto
    {
        public int TicketId { get; set; }

        [Required]
        public int ShowtimeId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(10)]
        public string? SeatNumber { get; set; }

        public DateTime? BookingTime { get; set; }

        public int? Status { get; set; }

        public int? OrderHistoryId { get; set; }

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