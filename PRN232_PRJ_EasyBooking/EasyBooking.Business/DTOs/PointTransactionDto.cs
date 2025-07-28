using System;

namespace EasyBooking.Business.DTOs
{
    public class PointTransactionDto
    {
        public int PointTransactionId { get; set; }
        public int UserId { get; set; }
        public int Points { get; set; }
        public string TransactionType { get; set; } // "EARN", "USE", "REDEEM"
        public string Description { get; set; }
        public int? OrderId { get; set; }
        public int? DiscountCodeId { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? DeleteAt { get; set; }
        public string DeleteBy { get; set; }
        public bool IsDelete { get; set; }

        // Additional properties for display
        public string TransactionTypeDisplay { get; set; }
        public string DiscountCodeName { get; set; }
    }
} 