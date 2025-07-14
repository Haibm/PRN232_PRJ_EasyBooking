using System;
using System.Collections.Generic;

namespace EasyBooking.Data.Entities;

public partial class Genre
{
    public int GenreId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
