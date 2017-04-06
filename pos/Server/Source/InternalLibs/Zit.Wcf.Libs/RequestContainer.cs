using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.Security;

namespace Zit.Wcf.Libs
{
    public class RequestContainer : IRequestContainer
    {
        public T Get<T>(string key) where T:class
        {
            WcfOperationContext ctx = WcfOperationContext.Current;
            if (ctx == null) throw new InvalidOperationException();

            if (ctx.Items.Contains(key))
            {
                return ctx.Items[key] as T;
            }
            else
                return null;

        }

        public void Set<T>(string key, T data) where T : class
        {
            WcfOperationContext ctx = WcfOperationContext.Current;
            if (ctx == null) throw new InvalidOperationException();

            if (ctx.Items.Contains(key))
            {
                ctx.Items[key] = data;
            }
            else
            {
                ctx.Items.Add(key, data);
            }
        }

        public bool Contain(string key)
        {
            WcfOperationContext ctx = WcfOperationContext.Current;
            if (ctx == null) throw new InvalidOperationException();
            return ctx.Items.Contains(key);
        }
    }
}
