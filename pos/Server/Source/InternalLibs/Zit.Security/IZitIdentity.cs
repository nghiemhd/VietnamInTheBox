using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Globalization;

namespace Zit.Security
{
    public interface IZitIdentity : IIdentity
    {
        string FullName { get; }
    }
}
