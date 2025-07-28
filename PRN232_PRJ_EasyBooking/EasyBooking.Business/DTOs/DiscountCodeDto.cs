using System;
using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Business.DTOs
{
    public class DiscountCodeDto
    {
        public int DiscountCodeId { get; set; }
        
        [Required(ErrorMessage = "Mã giảm giá là bắt buộc")]
        public string Code { get; set; }
        
        [Required(ErrorMessage = "Tên mã giảm giá là bắt buộc")]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [Range(0, 100, ErrorMessage = "Phần trăm giảm giá phải từ 0 đến 100")]
        public decimal DiscountPercentage { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Số tiền giảm giá không được âm")]
        public decimal DiscountAmount { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Điểm yêu cầu không được âm")]
        public int RequiredPoints { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Số lần sử dụng tối đa không được âm")]
        public int MaxUsage { get; set; }
        
        public int CurrentUsage { get; set; }
        
        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        public DateTime StartDate { get; set; }
        
        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        public DateTime EndDate { get; set; }
        
        public bool IsActive { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; } = "Staff";
        public DateTime? UpdateAt { get; set; }
        public string UpdateBy { get; set; } = "Staff";
        public DateTime? DeleteAt { get; set; }
        public string DeleteBy { get; set; } = "";
        public bool IsDelete { get; set; } = false;
    }
} 