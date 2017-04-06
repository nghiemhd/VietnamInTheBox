using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zit.Utils
{
    public static class BinaryHelper
    {
        public static string ToBase64(this byte[] binary)
        {
            if (binary == null) return null;
            return Convert.ToBase64String(binary);
        }
    }
}
