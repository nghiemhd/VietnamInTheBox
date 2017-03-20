using System;
using System.Collections.Generic;
using System.Text;
using Zit.Security;

namespace Zit.Cache
{
    public class SessionCache
    {
        public object GetAndReg(string key, Func<object> refreshAction)
        {
            if (refreshAction == null) throw new ArgumentNullException("refreshAction");

            object data = ZitSession.Current[key];

            if (data == null)
            {
                data = refreshAction();
                ZitSession.Current[key] = data;
            }
            return data;
        }

        public T GetAndReg<T>(string key, Func<T> refreshAction) where T : class
        {
            if (refreshAction == null) throw new ArgumentNullException("refreshAction");

            T data = ZitSession.Current[key] as T;

            if (data == null)
            {
                data = refreshAction();
                ZitSession.Current[key] = data;
            }
            return data;
        }

        public void Remove(string key)
        {
            ZitSession.Current[key] = null;
        }
    }
}
