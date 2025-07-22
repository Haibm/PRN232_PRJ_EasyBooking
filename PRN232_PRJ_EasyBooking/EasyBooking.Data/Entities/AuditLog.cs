using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class AuditLog
{
    public int LogId { get; set; }

    public int StaffId { get; set; }

    public DateTime ActionTime { get; set; }

    public virtual User Staff { get; set; } = null!;
}
