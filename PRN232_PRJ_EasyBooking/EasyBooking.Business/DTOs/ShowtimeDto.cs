using System;
using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Business.DTOs
{
    public class ShowtimeDto
    {
        public int ShowtimeId { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Range(0, 10000000)]
        public decimal Price { get; set; }

        [Range(0, 100)]
        public int? VipPercent { get; set; } // Phụ thu % giá ghế VIP

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