using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Business.DTOs
{
    public class SignupRequestDto
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; }
        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; }
        [Required]
        [StringLength(200)]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }
    }
} 