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

    public int? Status { get; set; }

    public int? OrderHistoryId { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? DeleteBy { get; set; }

    public DateTime? DeleteAt { get; set; }

    public bool? IsDelete { get; set; }

    public decimal? RefundAmount { get; set; }

    public DateTime? RefundTime { get; set; }

    public string? RefundReason { get; set; }

    public virtual OrderHistory? OrderHistory { get; set; }

    public virtual ICollection<RefundHistory> RefundHistories { get; set; } = new List<RefundHistory>();

    public virtual Showtime Showtime { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
