using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Business.DTOs
{
    public class GenreDto
    {
        public int GenreId { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

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