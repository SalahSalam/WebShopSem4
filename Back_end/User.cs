using System.Text;
using System.Text.RegularExpressions;

namespace WebShopSem4
{
    public enum Role
    {
        Admin,
        Customer
    }

    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public Role UserRole { get; set; }
        public string? Password { get; set; }
        public ShoppingCart Cart { get; private set; } = new ShoppingCart();
        private OrderRepository OrderRepository = new OrderRepository();

        public override string ToString()
        {
            return $"Id: {Id}, Username: {Username}, Role: {UserRole}";
        }
        private void ValidatePasswords()
        {
            if (Password == null)
                throw new ArgumentNullException("Password cannot be null");

            if (Password.Length < 8 || Password.Length > 64)
                throw new ArgumentOutOfRangeException("Password must be at least 8 and no more than 64 characters long");
        }
        private void ValidatePasswordComplexity()
        {
            if (!Regex.IsMatch(Password, @"[A-Z]"))
                throw new ArgumentException("Password must contain at least one uppercase letter");
            if (!Regex.IsMatch(Password, @"[a-z]"))
                throw new ArgumentException("Password must contain at least one lowercase letter");
            if (!Regex.IsMatch(Password, @"[0-9]"))
                throw new ArgumentException("Password must contain at least one digit");
            if (!Regex.IsMatch(Password, @"[\W_]"))
                throw new ArgumentException("Password must contain at least one special character");
        }
        public void ValidatePassword()
        {
            ValidatePasswords();
            ValidatePasswordComplexity();
        }
    }
}