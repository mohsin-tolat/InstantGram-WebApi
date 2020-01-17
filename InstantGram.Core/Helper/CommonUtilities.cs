using System;
using System.Security.Cryptography;

namespace InstantGram.Core.Helper
{
    public static class CommonUtilities
    {
        public static string GeneratePasswordSalt()
        {
            return Guid.NewGuid().ToString("N");
        }


        public static string GenerateHashPassword(string passwordValue, string passwordSalt)
        {
            var salt = System.Text.Encoding.UTF8.GetBytes(passwordSalt);
            var password = System.Text.Encoding.UTF8.GetBytes(passwordValue);

            var hmacMD5 = new HMACMD5(salt);
            var saltedHash = hmacMD5.ComputeHash(password);
            return Convert.ToBase64String(saltedHash);
        }
    }
}