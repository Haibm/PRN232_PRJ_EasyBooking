using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBooking.Business.DTOs
{
    public class MovieDetailDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
        public string? PosterUrl { get; set; }
        public string? Status { get; set; }
        public List<string> Genres { get; set; } = new();
        public List<ShowtimeDetailDto> Showtimes { get; set; } = new();
    }
}
