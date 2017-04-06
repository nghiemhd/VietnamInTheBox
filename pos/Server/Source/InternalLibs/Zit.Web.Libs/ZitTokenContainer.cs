using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using Zit.Security;
using System.Web;

namespace Zit.Web.Libs
{
    public class ZitTokenContainer : ITokenContainer
    {
        public string GetToken()
        {
            return "Zit";
        }

        public void SetToken(string token)
        {
        }

        public string Issue()
        {
            return "Zit";
        }

    }
}
