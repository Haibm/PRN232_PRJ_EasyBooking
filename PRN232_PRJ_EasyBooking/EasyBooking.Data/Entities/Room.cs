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

    public string? CreateBy { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? DeleteBy { get; set; }

    public DateTime? DeleteAt { get; set; }

    public bool? IsDelete { get; set; }

    public virtual Cinema Cinema { get; set; } = null!;

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
