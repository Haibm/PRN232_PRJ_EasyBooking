using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class OrderHistory
{
    public int OrderHistoryId { get; set; }

    public int PaymentId { get; set; }

    public int UserId { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? OrderNote { get; set; }

    public string? OrderStatus { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? DeleteBy { get; set; }

    public DateTime? DeleteAt { get; set; }

    public bool? IsDelete { get; set; }

    public virtual Payment Payment { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual User User { get; set; } = null!;
}
