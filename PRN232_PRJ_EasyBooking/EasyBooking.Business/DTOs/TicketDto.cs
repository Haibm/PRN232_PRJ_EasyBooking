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

    }
} 