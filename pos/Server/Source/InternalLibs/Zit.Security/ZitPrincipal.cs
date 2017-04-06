using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace Zit.Security
{
    public class ZitPrincipal : IZitPrincipal
    {
        private ZitIdentity _identity = null;
        HashSet<string> _roles = null;
        SortedSet<ZitMenu> _menus = null;

        public ZitPrincipal(IList<string> roles = null, string userName = null, string fullName = null, string authenticationType = null)
        {
            _identity = new ZitIdentity(userName, fullName, authenticationType);
            if (roles != null)
            {
                _roles = new HashSet<string>();
                foreach (var role in roles)
                    _roles.Add(role);
            }
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        public IZitIdentity ZitIdentity
        {
            get { return _identity; }
        }

        public bool IsInRole(string role)
        {
            return _roles.Contains(role);
        }

        public SortedSet<ZitMenu> Menus
        {
            get {
                return _menus;
            }
            set
            {
                _menus = value;
            }
        }

        public HashSet<string> Roles 
        {
            get
            {
                return _roles;
            }
        }
    }
}
