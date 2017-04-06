using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace Zit.Security
{
    public class ZitIdentity : IZitIdentity
    {
        private string _userName = null;
        private string _fullName = null;
        private string _authenticationType = null;

        public ZitIdentity(string userName, string fullName, string authenticationType)
        {
            _userName = userName;
            _fullName = fullName;
            _authenticationType = (authenticationType??"Zit");
        }

        public string AuthenticationType
        {
            get
            {
                return _authenticationType;
            }
        }

        public bool IsAuthenticated
        {
            get { return (_userName != null); }
        }

        public string Name
        {
            get { return _userName; }
        }

        public string FullName
        {
            get { return _fullName; }
        }
    }
}
