using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class Room
{
    public int RoomId { get; set; }

    public string Name { get; set; } = null!;

    public int Capacity { get; set; }

    public string? Location { get; set; }

    public int CinemaId { get; set; }

    public virtual Cinema Cinema { get; set; } = null!;

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
