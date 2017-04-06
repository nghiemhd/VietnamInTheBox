using System;
using System.Collections.Generic;
using System.Text;
using Zit.Security;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace Zit.Wcf.Libs
{
    public class ZitSessionContainer : ISessionContainer
    {
        public const string CACHENAME = "WCFSession";
        ICacheManager cache = ServiceLocator.Current.GetInstance<ICacheManager>(CACHENAME);
        ITokenContainer tokenCtn = ServiceLocator.Current.GetInstance<ITokenContainer>();
        public void SetSession(string token, ZitSession session)
        {
            cache.Add(token, session,CacheItemPriority.Normal, null, new SlidingTime(new TimeSpan(8,0,0)));
        }

        public ZitSession GetSession(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;

            var ss = cache.GetData(token) as ZitSession;
            if (ss != null && ss.IsAuthenticated)
            {
                var userName = ss.Principal.Identity.Name;
                if (cache.Contains(userName))
                {
                    DateTime expiredDate = (DateTime)cache.GetData(userName);
                    if(ss.CreatedDate < expiredDate)
                    {
                        cache.Remove(token);
                        return null;
                    }
                }
            }

            return cache.GetData(token) as ZitSession;
        }

        public void Remove(string token)
        {
            cache.Remove(token);
        }

        public void ExpireUser(string userName)
        {
            cache.Add(userName, DateTime.Now, CacheItemPriority.Normal, null, new SlidingTime(new TimeSpan(8, 0, 0)));
        }
    }
}
