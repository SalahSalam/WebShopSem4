using System.ComponentModel.DataAnnotations;

namespace Webshop.Shared.DTOs
{
    public class UserAuthDto
    {
        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required, MinLength(8), MaxLength(64)]
        public required string Password { get; set; }

        // FingerPrintJS string
        public required string VisitorId { get; set; }
    }
}
