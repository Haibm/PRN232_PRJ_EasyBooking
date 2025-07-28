using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class UserDiscountCode
{
    public int UserDiscountCodeId { get; set; }

    public int UserId { get; set; }

    public int DiscountCodeId { get; set; }

    public int PointsUsed { get; set; }

    public DateTime RedeemDate { get; set; }

    public bool IsUsed { get; set; }

    public DateTime? UsedDate { get; set; }

    public int? OrderId { get; set; }

    public DateTime CreateAt { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? DeleteAt { get; set; }

    public string? DeleteBy { get; set; }

    public bool IsDelete { get; set; }

    public virtual DiscountCode DiscountCode { get; set; } = null!;

    public virtual Payment? Order { get; set; }

    public virtual User User { get; set; } = null!;
}
