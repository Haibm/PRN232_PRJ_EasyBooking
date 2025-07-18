namespace EasyBooking.Business.DTOs
{
    public class ChangePasswordDto
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string VerificationCode { get; set; }
    }
} 