using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Business.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [StringLength(256)]
        public string? PasswordHash { get; set; }

        [StringLength(200)]
        public string? FullName { get; set; }

        [StringLength(50)]
        public string? Role { get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string? Email { get; set; }

        public bool? IsActive { get; set; }
    }
} 