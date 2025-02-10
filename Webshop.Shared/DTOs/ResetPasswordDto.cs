using System.ComponentModel.DataAnnotations;

namespace Webshop.Shared.DTOs
{
    public class ResetPasswordDto
    {
        [Required]
        public required string Token { get; set; }

        [Required, MinLength(8), MaxLength(64)]
        public required string NewPassword { get; set; }

        public string? VisitorId { get; set; }
    }
}
