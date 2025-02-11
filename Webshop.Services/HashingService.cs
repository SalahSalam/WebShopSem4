using Isopoh.Cryptography.Argon2;
using System.Security.Cryptography;
using System.Text;

namespace Webshop.Services
{
    public class HashingService
    {
        public string GenerateHash(string password)
        {
            return Argon2.Hash(password);
        }

        public bool VerifyHash(string password, string hash)
        {
            return Argon2.Verify(hash, password);
        }
        public string ComputeSha1Hash(string input)
        {
            using var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
        }

        public string ComputeSha256Hash(string input)
        {
            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
