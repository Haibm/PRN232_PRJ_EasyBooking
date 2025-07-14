using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int TicketId { get; set; }

    public decimal Amount { get; set; }

    public DateTime? PaymentTime { get; set; }

    public string? TransactionId { get; set; }

    public string? Status { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;
}
