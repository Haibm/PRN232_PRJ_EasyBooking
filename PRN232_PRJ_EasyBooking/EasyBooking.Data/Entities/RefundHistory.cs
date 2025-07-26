using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class RefundHistory
{
    public int RefundHistoryId { get; set; }

    public int TicketId { get; set; }

    public decimal RefundAmount { get; set; }

    public DateTime RefundTime { get; set; }

    public string? Reason { get; set; }

    public int UserId { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
