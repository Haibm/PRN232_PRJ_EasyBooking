using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Business.DTOs
{
    public class GenreDto
    {
        public int GenreId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
} 