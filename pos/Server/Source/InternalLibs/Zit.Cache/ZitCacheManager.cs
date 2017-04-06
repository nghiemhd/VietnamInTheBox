using System;
using System.Collections.Generic;
using System.Text;
using Zit.Security;
using Microsoft.Practices.ServiceLocation;

namespace Zit.Cache
{
    public class ZitCacheManager
    {
        public SessionCache SessionCache {
            get
            {
                return ServiceLocator.Current.GetInstance<SessionCache>();
            }
        }

        public GlobalCache GlobalCache
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GlobalCache>();
            }
        }

        public static ZitCacheManager Current
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ZitCacheManager>();
            }
        }
    }
}
