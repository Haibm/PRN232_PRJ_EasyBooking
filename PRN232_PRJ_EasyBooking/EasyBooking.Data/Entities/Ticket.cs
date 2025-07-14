using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int ShowtimeId { get; set; }

    public int UserId { get; set; }

    public string SeatNumber { get; set; } = null!;

    public DateTime? BookingTime { get; set; }

    public string? Status { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Showtime Showtime { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
