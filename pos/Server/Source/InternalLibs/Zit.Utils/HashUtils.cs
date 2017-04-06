using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Zit.Utils
{
    public static class HashUtils
    {
        public static string GetMd5Hash(params string[] arrParams)
        {
            string Input = "";
            int i = 0;
            for (i = 0; i < arrParams.Length; i++)
            {
                Input = Input + " " + arrParams[i];
            }


            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();

            byte[] bs = System.Text.Encoding.UTF8.GetBytes(Input);

            bs = x.ComputeHash(bs);

            System.Text.StringBuilder s = new System.Text.StringBuilder();

            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string md5String = s.ToString();
            return md5String;
        }
    }
}
