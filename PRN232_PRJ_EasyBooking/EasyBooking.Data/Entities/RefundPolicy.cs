using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class RefundPolicy
{
    public int RefundPolicyId { get; set; }

    public string PolicyName { get; set; } = null!;

    public decimal RefundPercentage { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? DeleteBy { get; set; }

    public DateTime? DeleteAt { get; set; }

    public bool? IsDelete { get; set; }
}
