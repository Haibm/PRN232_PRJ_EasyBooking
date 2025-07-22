using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class Seat
{
    public int SeatId { get; set; }

    public int RoomId { get; set; }

    public string RowLetter { get; set; } = null!;

    public int SeatNumber { get; set; }

    public string SeatType { get; set; } = null!;

    public string? CreateBy { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? DeleteBy { get; set; }

    public DateTime? DeleteAt { get; set; }

    public bool? IsDelete { get; set; }

    public virtual Room Room { get; set; } = null!;
}
