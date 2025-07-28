using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class PointConfig
{
    public int PointConfigId { get; set; }

    public string ConfigName { get; set; } = "";

    public string Description { get; set; } = "";

    public int AmountPerPoint { get; set; }

    public decimal MinAmountToEarn { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreateAt { get; set; } = DateTime.Now;

    public string CreateBy { get; set; } = "";

    public DateTime? UpdateAt { get; set; }

    public string UpdateBy { get; set; } = "";

    public DateTime? DeleteAt { get; set; }

    public string DeleteBy { get; set; } = "";

    public bool IsDelete { get; set; } = false;
}
