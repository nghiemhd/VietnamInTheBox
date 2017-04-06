using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.Security;
using System.Web;

namespace Zit.Web.Libs
{
    public class ZitSessionContainer : ISessionContainer
    {
        public void SetSession(string token, ZitSession session)
        {
            if (HttpContext.Current.Session == null) return;
            HttpContext.Current.Session[token] = session;
        }

        public ZitSession GetSession(string token)
        {
            if (HttpContext.Current.Session == null) return null;
            return HttpContext.Current.Session[token] as ZitSession;
        }


        public void Remove(string token)
        {
            HttpContext.Current.Session.Remove(token);
        }

        public void ExpireUser(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
