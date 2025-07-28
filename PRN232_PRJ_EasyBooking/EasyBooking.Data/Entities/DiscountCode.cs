using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class DiscountCode
{
    public int DiscountCodeId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal DiscountPercentage { get; set; }

    public decimal DiscountAmount { get; set; }

    public int RequiredPoints { get; set; }

    public int MaxUsage { get; set; }

    public int CurrentUsage { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateAt { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? DeleteAt { get; set; }

    public string? DeleteBy { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<PointTransaction> PointTransactions { get; set; } = new List<PointTransaction>();

    public virtual ICollection<UserDiscountCode> UserDiscountCodes { get; set; } = new List<UserDiscountCode>();
}
