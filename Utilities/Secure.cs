using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Secure
    {
        public static string EncryptString(string value)
        {
            string saltPassword = "0&!mRUpo1&SqKDlni78**Z4^vlw15pdpRJ!j52MiUSAKp16TyK";
            byte[] salt = Encoding.UTF8.GetBytes(saltPassword);
            byte[] passwordByte = Encoding.UTF8.GetBytes(value);
            var hmacSHA256 = new HMACSHA256(salt);
            var saltedHash = hmacSHA256.ComputeHash(passwordByte);
            return Convert.ToBase64String(saltedHash);
        }
    }
}
