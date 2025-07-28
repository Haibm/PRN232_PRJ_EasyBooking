using System;

namespace EasyBooking.Data.Entities
{
    public class UserPoints
    {
        public int UserPointsId { get; set; }
        public int UserId { get; set; }
        public int CurrentPoints { get; set; }
        public int TotalEarnedPoints { get; set; }
        public int TotalUsedPoints { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? DeleteAt { get; set; }
        public string DeleteBy { get; set; }
        public bool IsDelete { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
} 