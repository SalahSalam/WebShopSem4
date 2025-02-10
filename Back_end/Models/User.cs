namespace WebShop.Repos.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public DateTime CreatedAt { get; internal set; } = DateTime.UtcNow;
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiration { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Username: {Username}";
        }
    }
}