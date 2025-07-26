using System;
using System.Collections.Generic;

namespace EasyBooking.Business.DTOs
{
    public class MovieDto
    {
        public int MovieId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
        public string? PosterUrl { get; set; }
        public int? Status { get; set; }
        public List<string> Genres { get; set; } = new();
        public List<DateTime> Showtimes { get; set; } = new();

        // Thêm các trường quản lý
        public bool? IsDelete { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string? DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
    }
} 