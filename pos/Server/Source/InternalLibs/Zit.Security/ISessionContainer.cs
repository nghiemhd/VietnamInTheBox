using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zit.Security
{
    public interface ISessionContainer
    {
        void SetSession(string token, ZitSession session);
        ZitSession GetSession(string token);
        void Remove(string token);
        void ExpireUser(string userName);
    }
}
