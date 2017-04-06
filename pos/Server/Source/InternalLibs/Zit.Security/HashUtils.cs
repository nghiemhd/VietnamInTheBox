using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magnum.Cryptography;

namespace Zit.Security
{
    public static class HashUtils
    {
        public static string Hash(this string text, string salt = null)
        {
            text = (text ?? "");
            salt = (salt ?? "");
            var hashService = new Sha512HMacHashingService(salt);
            return hashService.Hash(text);
        }

        public static bool VerifyHash(this string hashed, string text,string salt = null)
        {
            text = (text ?? "");
            salt = (salt ?? "");
            var hashService = new Sha512HMacHashingService(salt);
            return hashService.Hash(text) == hashed;
        }
    }
}
