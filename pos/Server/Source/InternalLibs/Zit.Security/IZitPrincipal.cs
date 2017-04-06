using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace Zit.Security
{
    public interface IZitPrincipal : IPrincipal
    {
        SortedSet<ZitMenu> Menus { get; set; }
        HashSet<string> Roles { get; }
        IZitIdentity ZitIdentity { get; }
    }
}
