using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public decimal Amount { get; set; }

    public DateTime? PaymentTime { get; set; }

    public string? TransactionId { get; set; }

    public bool? Status { get; set; }

    public int? UserId { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? DeleteBy { get; set; }

    public DateTime? DeleteAt { get; set; }

    public bool? IsDelete { get; set; }

    public string? SeatsJson { get; set; }

    public int? ShowtimeId { get; set; }

    public virtual ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();

    public virtual User? User { get; set; }
}
