using System;

namespace EasyBooking.Business.DTOs
{
    public class RefundPolicyDto
    {
        public int RefundPolicyId { get; set; }
        
        public string PolicyName { get; set; } = null!;
        
        public decimal RefundPercentage { get; set; }
        
        public string? Description { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime? CreateAt { get; set; }
        
        public string? CreateBy { get; set; }
        
        public DateTime? UpdateAt { get; set; }
        
        public string? UpdateBy { get; set; }
        
        public DateTime? DeleteAt { get; set; }
        
        public string? DeleteBy { get; set; }
        
        public bool? IsDelete { get; set; }
    }
} 