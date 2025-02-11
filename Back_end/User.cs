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
        private void ValidatePasswords(string password)
        {
            if (password == null)
                throw new ArgumentNullException("Password cannot be null");

            if (password.Length < 8 || password.Length > 64)
                throw new ArgumentOutOfRangeException("Password must be bewtween 8 and 64 characters long");
        }
        private void ValidatePasswordComplexity(string password)
        {
            if (!Regex.IsMatch(password, @"[A-Z]"))
                throw new ArgumentException("Password must contain at least one uppercase letter");
            if (!Regex.IsMatch(password, @"[a-z]"))
                throw new ArgumentException("Password must contain at least one lowercase letter");
            if (!Regex.IsMatch(password, @"[0-9]"))
                throw new ArgumentException("Password must contain at least one digit");
            if (!Regex.IsMatch(password, @"[\W_]"))
                throw new ArgumentException("Password must contain at least one special character");
        }
        public void ValidatePassword(string password)
        {
            ValidatePasswords(password);
            ValidatePasswordComplexity(password);
        }
        public string ChangePass(string password)
        {
            ValidatePassword(password);
            Password = password;

            return Password;
        }
        public void ValidateId()
        {
            if (Id < 0)
            {
                throw new ArgumentException("Id must be greater than 0");
            }
        }

    }
}