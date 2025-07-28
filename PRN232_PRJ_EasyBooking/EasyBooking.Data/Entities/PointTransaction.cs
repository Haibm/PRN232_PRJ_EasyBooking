using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class PointTransaction
{
    public int PointTransactionId { get; set; }

    public int UserId { get; set; }

    public int Points { get; set; }

    public string TransactionType { get; set; } = null!;

    public string? Description { get; set; }

    public int? OrderId { get; set; }

    public int? DiscountCodeId { get; set; }

    public DateTime TransactionDate { get; set; }

    public DateTime CreateAt { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? DeleteAt { get; set; }

    public string? DeleteBy { get; set; }

    public bool IsDelete { get; set; }

    public virtual DiscountCode? DiscountCode { get; set; }

    public virtual Payment? Order { get; set; }

    public virtual User User { get; set; } = null!;
}
