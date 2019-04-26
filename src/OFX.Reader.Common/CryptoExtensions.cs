using System;
using System.Security.Cryptography;
using System.Text;

namespace OFX.Reader.Common {

    public static class CryptoExtensions {

        public static string ToHashHMAC(this string key, string message) {
            
            ASCIIEncoding encoding = new ASCIIEncoding();
            
            byte[] encodedKey = encoding.GetBytes(key);
            byte[] encodedMessage = encoding.GetBytes(message);
            
            HMACSHA256 hash = new HMACSHA256(encodedKey);
            byte[] encryptedHash = hash.ComputeHash(encodedMessage);
            
            return BitConverter.ToString(encryptedHash).Replace("-", "").ToLower();
        }
    }

}