using System;

namespace EasyBooking.Business.DTOs
{
    public class UserPointsDto
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
    }
} 