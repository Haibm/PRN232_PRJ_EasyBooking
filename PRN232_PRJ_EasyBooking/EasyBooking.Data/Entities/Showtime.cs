﻿using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class Showtime
{
    public int ShowtimeId { get; set; }

    public int MovieId { get; set; }

    public int RoomId { get; set; }

    public DateTime StartTime { get; set; }

    public decimal Price { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
