using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class Cinema
{
    public int CinemaId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
