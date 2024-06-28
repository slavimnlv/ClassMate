using ClassMate.Domain.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Services
{
    public class PasswordService : IPasswordService
    {
        public void HashPassword(string password, out string hash, out string salt)
        {
            byte[] saltArr = RandomNumberGenerator.GetBytes(64);
            byte[] pass = Encoding.UTF8.GetBytes(password);

            byte[] hashArr = hasher(pass, saltArr);

            salt = Convert.ToBase64String(saltArr);
            hash = Convert.ToBase64String(hashArr);
        }

        public bool VerifyHashedPassword(string providedPassword, string hashedPassword, string saltString)
        {
            byte[] salt = Convert.FromBase64String(saltString);
            byte[] password = Encoding.UTF8.GetBytes(providedPassword);


            byte[] hashToCompare = hasher(password, salt);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromBase64String(hashedPassword));
        }

        private byte[] hasher(byte[] password, byte[] salt)
        {
            return Rfc2898DeriveBytes.Pbkdf2(password, salt, 350000, HashAlgorithmName.SHA512, 64);
        }


    }
}
