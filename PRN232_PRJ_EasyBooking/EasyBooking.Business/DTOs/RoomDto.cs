using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Business.DTOs
{
    public class RoomDto
    {
        public int RoomId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(1, 1000)]
        public int Capacity { get; set; }

        [StringLength(200)]
        public string? Location { get; set; }

        [Required]
        public int CinemaId { get; set; }
    }
} 