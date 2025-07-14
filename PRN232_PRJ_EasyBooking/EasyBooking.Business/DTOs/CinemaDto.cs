using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Business.DTOs
{
    public class CinemaDto
    {
        public int CinemaId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(300)]
        public string Address { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }
    }
} 