using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace InstantGram.Common.Helper
{
    public static class CommonUtilities
    {
        public static string GeneratePasswordSalt()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static string GenerateRandomString()
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

        public static DateTime GetCurrentDateTime()
        {
            return DateTime.UtcNow;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }

            var collection = enumerable as ICollection<T>;
            if (collection != null)
            {
                return collection.Count < 1;
            }
            return !enumerable.Any();
        }


    }
}