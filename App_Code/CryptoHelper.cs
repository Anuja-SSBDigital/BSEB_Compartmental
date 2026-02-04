using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class CryptoHelper
{
    // Use a strong key & IV (store securely, e.g., in web.config)
    private static readonly string EncryptionKey = "MySecretKey12345"; // must be 16/24/32 chars
    private static readonly byte[] IV = { 12, 34, 56, 78, 90, 123, 231, 111, 54, 88, 99, 101, 111, 222, 155, 11 };

    public static string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
            aes.IV = IV;

            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                using (StreamWriter sw = new StreamWriter(cs))
                    sw.Write(plainText);

                string base64 = Convert.ToBase64String(ms.ToArray());
                return UrlEncodeBase64(base64); // make URL-safe
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        string base64 = UrlDecodeBase64(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
            aes.IV = IV;

            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64)))
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }

    private static string UrlEncodeBase64(string base64)
    {
        return base64.Replace("+", "-")
                     .Replace("/", "_")
                     .Replace("=", "");
    }

    private static string UrlDecodeBase64(string base64Url)
    {
        string base64 = base64Url.Replace("-", "+").Replace("_", "/");
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return base64;
    }

}
