using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Business.DTOs
{
    public class SignupVerifyDto
    {
        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }
        [Required]
        [StringLength(10)]
        public string Otp { get; set; }
    }
} 