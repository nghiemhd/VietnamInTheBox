using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Zit.Utils
{
    public static class NetworkHelper
    {
        public static string[] GetIPAddress()
        {
            string host = Dns.GetHostName();
            IPHostEntry ip = Dns.GetHostEntry(host);
            return ip.AddressList.Where(m => m.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(k => k.ToString()).ToArray();
        }
    }
}
