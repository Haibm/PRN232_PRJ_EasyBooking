using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Business.DTOs
{
    public class CinemaDto
    {
        public int CinemaId { get; set; }

        [Required(ErrorMessage = "Tên rạp là bắt buộc")]
        [StringLength(200)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        [StringLength(300)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

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