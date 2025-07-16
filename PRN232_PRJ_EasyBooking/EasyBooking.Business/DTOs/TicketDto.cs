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
        public string SeatNumber { get; set; }

        public DateTime? BookingTime { get; set; }

        [StringLength(20)]
        public string? Status { get; set; }

        [StringLength(20)]
        public string? PaymentMethod { get; set; }
    }
}
