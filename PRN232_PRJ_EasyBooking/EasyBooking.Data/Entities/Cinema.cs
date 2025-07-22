using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class Cinema
{
    public int CinemaId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Phone { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? DeleteBy { get; set; }

    public DateTime? DeleteAt { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
