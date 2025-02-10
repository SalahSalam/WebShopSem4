using System.ComponentModel.DataAnnotations;

namespace Webshop.Shared.DTOs
{
    public class UserDto
    {
        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Username { get; set; }

        // FingerPrintJS string
        [Required]
        public required string VisitorId { get; set; }
    }
}
