using System;
using System.Collections.Generic;

namespace EasyBooking.Business.DTOs
{
    public class MovieDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
        public string? PosterUrl { get; set; }
        public string? Status { get; set; }
        public List<string> Genres { get; set; } = new();
        public List<DateTime> Showtimes { get; set; } = new();
    }
} 