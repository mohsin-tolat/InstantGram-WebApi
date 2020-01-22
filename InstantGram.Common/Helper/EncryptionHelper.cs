using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace InstantGram.Common.Helper
{
    public static class EncryptionHelper
    {

        public const string ENCRYPTION_KEY = "C&F)J@NcRfUjMn2r";

        public static string ToEncrypt(this object plainText)
        {
            if (plainText != null && string.IsNullOrWhiteSpace(plainText.ToString()))
            {
                return string.Empty;
            }

            var _key = Encoding.UTF8.GetBytes(ENCRYPTION_KEY);

            using (var aes = Aes.Create())
            {
                using (var encryptor = aes.CreateEncryptor(_key, aes.IV))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (var sw = new StreamWriter(cs))
                            {
                                sw.Write(plainText);
                            }
                        }

                        var iv = aes.IV;

                        var encrypted = ms.ToArray();

                        var result = new byte[iv.Length + encrypted.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static string ToDecrypt(this string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
            {
                return encryptedText;
            }

            var b = Convert.FromBase64String(encryptedText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(b, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(b, iv.Length, cipher, 0, iv.Length);

            var _key = Encoding.UTF8.GetBytes(ENCRYPTION_KEY);

            using (var aes = Aes.Create())
            {
                using (var decryptor = aes.CreateDecryptor(_key, iv))
                {
                    var result = string.Empty;
                    using (var ms = new MemoryStream(cipher))
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (var sr = new StreamReader(cs))
                            {
                                result = sr.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
    }
}