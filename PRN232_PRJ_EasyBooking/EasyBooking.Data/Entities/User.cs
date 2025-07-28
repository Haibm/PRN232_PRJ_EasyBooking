using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Role { get; set; }

    public string? Email { get; set; }

    public bool? IsActive { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? DeleteBy { get; set; }

    public DateTime? DeleteAt { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<PointTransaction> PointTransactions { get; set; } = new List<PointTransaction>();

    public virtual ICollection<RefundHistory> RefundHistories { get; set; } = new List<RefundHistory>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<UserDiscountCode> UserDiscountCodes { get; set; } = new List<UserDiscountCode>();

    public virtual ICollection<UserPoints> UserPoints { get; set; } = new List<UserPoints>();
}
