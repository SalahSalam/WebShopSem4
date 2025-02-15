using System.Text.RegularExpressions;

namespace Webshop.Services
{
    public class ValidationService
    {
        // TODO: Rendundant due to modelstate validatio in #UsersController. Remove or keep for redundancy?
        public bool IsEmailValid(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        // TODO: Rendundant due to modelstate validatio in #UsersController. Remove or keep for redundancy?
        public bool IsPasswordValidLength(string password)
        {
            return password.Length >= 8 &&
                   password.Length <= 64;
        }
    }
}
