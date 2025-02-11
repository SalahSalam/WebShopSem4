using System.ComponentModel.DataAnnotations;

namespace Webshop.Shared.DTOs
{
    public class UserDto
    {
        public required int Id { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }
    }
}
