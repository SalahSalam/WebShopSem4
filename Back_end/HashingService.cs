using Isopoh.Cryptography.Argon2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSem4
{
    public class HashingService
    {
        public string HashPassword(string password) 
        {
            return Argon2.Hash(password);
        }
        public bool VerifyPassword(string password, string hash)
        {
            return Argon2.Verify(password, hash);
        }
    }
}
