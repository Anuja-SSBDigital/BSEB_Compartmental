using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

/// <summary>
/// Summary description for Encryption
/// </summary>
public class Encryption
{
    private const int IV_SIZE = 12;  // 96 bits for AES-GCM
    private const int TAG_SIZE = 16; // 128 bits for the authentication tag
    private const int HMAC_LENGTH = 48; // SHA-384 = 48 bytes

    // Convert Base64 string to byte array (for keys)
    private static byte[] Base64ToBytes(string base64Key)
    {
        return Convert.FromBase64String(base64Key);
    }

    // Convert byte array to Hex string
    private static string BytesToHex(byte[] bytes)
    {
        StringBuilder sb = new StringBuilder(bytes.Length * 2);
        foreach (byte b in bytes)
        {
            sb.AppendFormat("{0:X2}", b);
        }
        return sb.ToString();
    }

    // Generate a unique transaction ID using GUID
    public string randomTxnId()
    {
        return Guid.NewGuid().ToString("N");  // Generates a unique GUID string without hyphens
    }

    // Parse query string to Dictionary
    public Dictionary<string, string> queryParser(string values)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        string[] sites = values.Split('&');
        string[] token;

        foreach (string s in sites)
        {
            token = s.Split('=');
            if (token.Length == 2) // Ensure both key and value exist
            {
                dict.Add(token[0], token[1]);
            }
        }
        return dict;
    }

    // Encrypt the plaintext using AES-GCM and HMAC-SHA384
    public string EncryptString(string aesKeyBase64, string hmacKeyBase64, string plaintext)
    {
        byte[] aesKey = Base64ToBytes(aesKeyBase64);  // Decode AES key from Base64
        byte[] hmacKey = Base64ToBytes(hmacKeyBase64);  // Decode HMAC key from Base64

        // Generate random IV (12 bytes, 96 bits)
        byte[] iv = new byte[IV_SIZE];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(iv);  // Generate a random IV
        }

        // AES-GCM Encryption using BouncyCastle
        var gcmBlockCipher = new GcmBlockCipher(new AesEngine());
        var parameters = new AeadParameters(new KeyParameter(aesKey), TAG_SIZE * 8, iv); // Tag size in bits
        gcmBlockCipher.Init(true, parameters);  // true for encryption
        byte[] encryptedBytes = new byte[plaintext.Length + TAG_SIZE];
        int outputLen = gcmBlockCipher.ProcessBytes(Encoding.UTF8.GetBytes(plaintext), 0, plaintext.Length, encryptedBytes, 0);
        gcmBlockCipher.DoFinal(encryptedBytes, outputLen);

        // Generate HMAC over IV + Ciphertext + Tag
        byte[] cipherTextWithTag = new byte[IV_SIZE + encryptedBytes.Length];
        Buffer.BlockCopy(iv, 0, cipherTextWithTag, 0, IV_SIZE);
        Buffer.BlockCopy(encryptedBytes, 0, cipherTextWithTag, IV_SIZE, encryptedBytes.Length);

        using (var hmacSha384 = new HMACSHA384(hmacKey))
        {
            byte[] hmac = hmacSha384.ComputeHash(cipherTextWithTag);

            // Combine HMAC + IV + Ciphertext + Tag for the final message
            byte[] finalMessage = new byte[hmac.Length + cipherTextWithTag.Length];
            Buffer.BlockCopy(hmac, 0, finalMessage, 0, hmac.Length);
            Buffer.BlockCopy(cipherTextWithTag, 0, finalMessage, hmac.Length, cipherTextWithTag.Length);

            return BytesToHex(finalMessage);  // Return the encrypted message in HEX format
        }
    }

    // Decrypt the ciphertext using AES-GCM and verify HMAC
    public string DecryptString(string aesKeyBase64, string hmacKeyBase64, string hexCipherText)
    {
        byte[] aesKey = Base64ToBytes(aesKeyBase64);  // Decode AES key from Base64
        byte[] hmacKey = Base64ToBytes(hmacKeyBase64);  // Decode HMAC key from Base64

        // Convert the hex ciphertext to byte array
        byte[] fullMessage = HexToBytes(hexCipherText);

        // Split the HMAC from the rest of the message
        byte[] hmacReceived = new byte[HMAC_LENGTH];
        byte[] encryptedData = new byte[fullMessage.Length - HMAC_LENGTH];
        Buffer.BlockCopy(fullMessage, 0, hmacReceived, 0, HMAC_LENGTH);
        Buffer.BlockCopy(fullMessage, HMAC_LENGTH, encryptedData, 0, encryptedData.Length);

        // Verify HMAC
        using (var hmacSha384 = new HMACSHA384(hmacKey))
        {
            byte[] computedHmac = hmacSha384.ComputeHash(encryptedData);
            if (!AreByteArraysEqual(hmacReceived, computedHmac))
            {
                throw new CryptographicException("HMAC validation failed. Data may have been tampered.");
            }
        }

        // Extract the IV, Ciphertext, and Tag
        byte[] iv = new byte[IV_SIZE];
        Buffer.BlockCopy(encryptedData, 0, iv, 0, IV_SIZE);

        byte[] cipherTextWithTag = new byte[encryptedData.Length - IV_SIZE];
        Buffer.BlockCopy(encryptedData, IV_SIZE, cipherTextWithTag, 0, cipherTextWithTag.Length);

        // AES-GCM Decryption using BouncyCastle
        var gcmBlockCipher = new GcmBlockCipher(new AesEngine());
        var parameters = new AeadParameters(new KeyParameter(aesKey), TAG_SIZE * 8, iv);  // Tag size in bits
        gcmBlockCipher.Init(false, parameters);  // false for decryption
        byte[] plainBytes = new byte[gcmBlockCipher.GetOutputSize(cipherTextWithTag.Length)];
        int outputLen = gcmBlockCipher.ProcessBytes(cipherTextWithTag, 0, cipherTextWithTag.Length, plainBytes, 0);
        gcmBlockCipher.DoFinal(plainBytes, outputLen);

        return Encoding.UTF8.GetString(plainBytes);
    }

    // Utility method to compare byte arrays
    private static bool AreByteArraysEqual(byte[] a, byte[] b)
    {
        if (a.Length != b.Length) return false;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i]) return false;
        }
        return true;
    }

    // Utility method to convert HEX string to byte array
    private static byte[] HexToBytes(string hex)
    {
        int len = hex.Length;
        byte[] data = new byte[len / 2];
        for (int i = 0; i < len; i += 2)
        {
            data[i / 2] = (byte)((Convert.ToInt32(hex[i].ToString(), 16) << 4)
                                 + Convert.ToInt32(hex[i + 1].ToString(), 16));
        }
        return data;
    }
}
