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

    public virtual Room Room { get; set; } = null!;
}
