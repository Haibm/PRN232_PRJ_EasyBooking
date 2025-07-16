using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Business.DTOs
{
    public class RoomDto
    {
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Tên phòng là bắt buộc")]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(1, 1000, ErrorMessage = "Sức chứa phải lớn hơn 0")]
        public int Capacity { get; set; }

        [StringLength(200)]
        public string? Location { get; set; }

        [Required(ErrorMessage = "Phải chọn rạp")]
        public int CinemaId { get; set; }
    }
} 