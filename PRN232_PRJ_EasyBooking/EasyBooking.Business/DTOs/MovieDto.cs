using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Business.DTOs
{
    public class MovieDto
    {
        public int MovieId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(1, 500)]
        public int? Duration { get; set; }

        [Url]
        public string? PosterUrl { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }
    }
} 