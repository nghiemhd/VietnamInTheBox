using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.ServiceLocation;

namespace Zit.Cache
{
    public class GlobalCache
    {
        public const string CACHENAME = "GLOBALCACHE";
        ICacheManager cache = ServiceLocator.Current.GetInstance<ICacheManager>(CACHENAME);

        private object __getAndReg(string key, Expiration exp, int expSecond, Func<object> refreshAction)
        {
            if (refreshAction == null) throw new ArgumentNullException("refreshAction");

            object data = cache.GetData(key);
            if (data == null && (!cache.Contains(key)))
            {
                data = refreshAction();
                Add(key, data, exp, expSecond, refreshAction);
            }
            return data;
        }

        public object GetAndReg(string key,Expiration exp, int expSecond, Func<object> refreshAction)
        {
            return __getAndReg(key, exp, expSecond, refreshAction);
        }

        public T GetAndReg<T>(string key, Expiration exp, int expSecond, Func<T> refreshAction) where T : class
        {
            object ret = __getAndReg(key, exp, expSecond, refreshAction);
            return ret as T;
        }

        public void Add(string key,object obj,Expiration exp,int expSecond, Func<object> refreshAction)
        {
            if (cache.Contains(key))
                cache.Remove(key);
            CacheRefresh cacheRefresh = null;
            if(refreshAction != null) 
                cacheRefresh = new CacheRefresh(refreshAction,cache);

            ICacheItemExpiration cacheExpiration = null;
            switch(exp)
            {
                case Expiration.Absolute:
                    cacheExpiration = new AbsoluteTime(new TimeSpan(0,0,expSecond));
                    break;
                case Expiration.Slide:
                    cacheExpiration = new SlidingTime(new TimeSpan(0,0,expSecond));
                    break;
                case Expiration.Never:
                    cacheExpiration = new NeverExpired();
                    break;
            }

            cache.Add(key, obj, CacheItemPriority.Normal, cacheRefresh, cacheExpiration);
        }

        public object Get(string key, object obj)
        {
            return cache.GetData(key);
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }
    }

    public enum Expiration
    {
        Absolute,
        Slide,
        Never
    }

    public class CacheRefresh : ICacheItemRefreshAction
    {
        private ICacheManager _cacheManager;
        private Func<object> _refreshAction;
        public CacheRefresh(Func<object> refreshAction,ICacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
            this._refreshAction = refreshAction;
        }

        public void Refresh(string removedKey, object expiredValue, CacheItemRemovedReason removalReason)
        {
            this._cacheManager.Add(removedKey, _refreshAction());
        }
    }
}
