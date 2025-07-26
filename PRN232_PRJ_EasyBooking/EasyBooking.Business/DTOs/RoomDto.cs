using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Business.DTOs
{
    public class RoomDto
    {
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Tên phòng là bắt buộc")]
        [StringLength(100)]
        public string? Name { get; set; }

        [Range(1, 1000, ErrorMessage = "Sức chứa phải lớn hơn 0")]
        public int Capacity { get; set; }

        [StringLength(200)]
        public string? Location { get; set; }

        [Required(ErrorMessage = "Phải chọn rạp")]
        public int CinemaId { get; set; }

        // Trường mới đồng bộ với DB
        public int RowCount { get; set; }
        public int ColCount { get; set; }
        public string? RoomType { get; set; }
        public string? CinemaName { get; set; } // chỉ để hiển thị, không lưu DB

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