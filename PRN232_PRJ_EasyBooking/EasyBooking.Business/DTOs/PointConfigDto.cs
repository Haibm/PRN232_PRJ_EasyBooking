using System;
using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Business.DTOs
{
    public class PointConfigDto
    {
        public int PointConfigId { get; set; }
        
        [Required(ErrorMessage = "Tên cấu hình là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên cấu hình không được quá 100 ký tự")]
        public string ConfigName { get; set; } = "";
        
        [StringLength(255, ErrorMessage = "Mô tả không được quá 255 ký tự")]
        public string Description { get; set; } = "";
        
        [Required(ErrorMessage = "Số điểm là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số điểm phải lớn hơn 0")]
        public int AmountPerPoint { get; set; } // Số điểm được tích
        
        [Required(ErrorMessage = "Số tiền tối thiểu là bắt buộc")]
        [Range(1, double.MaxValue, ErrorMessage = "Số tiền tối thiểu phải lớn hơn 0")]
        public decimal MinAmountToEarn { get; set; } // Số tiền tối thiểu để được tích điểm
        
        public bool IsActive { get; set; } = true;
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string CreateBy { get; set; } = "";
        public DateTime? UpdateAt { get; set; }
        public string UpdateBy { get; set; } = "";
        public DateTime? DeleteAt { get; set; }
        public string DeleteBy { get; set; } = "";
        public bool IsDelete { get; set; } = false;
    }
} 