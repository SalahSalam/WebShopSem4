using System.Text.RegularExpressions;

namespace Webshop.Services
{
    public class ValidationService
    {
        public bool IsEmailValid(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        public bool IsPasswordValidLength(string password)
        {
            return password.Length >= 8 &&
                   password.Length <= 64;
        }


        public bool IsPasswordStrong(string password)
        {
            var score = Zxcvbn.Core.EvaluatePassword(password).Score; // Returns 0-4: very weak to very strong
            return score >= 2; // must be atleast fair
        }
    }
}
