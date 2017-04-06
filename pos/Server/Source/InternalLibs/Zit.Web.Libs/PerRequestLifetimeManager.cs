using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;

namespace Zit.Web.Libs
{
    public class PerRequestLifetimeManager : LifetimeManager
    {
        private readonly Guid key;

        public PerRequestLifetimeManager()
        {
            key = Guid.NewGuid();
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            if (HttpContext.Current == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Items[key];
            }
        }

        /// <summary>
        /// Removes the value.
        /// </summary>
        public override void RemoveValue()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items.Remove(key);
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        public override void SetValue(object newValue)
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Items.Contains(key))
                    HttpContext.Current.Items[key] = newValue;
                else
                {
                    HttpContext.Current.Items.Add(key, newValue);

                    List<Guid> listKey = null;

                    if(HttpContext.Current.Items.Contains("PerRequestLifetimeManager"))
                    {
                        listKey = (List<Guid>)HttpContext.Current.Items["PerRequestLifetimeManager"];
                    }
                    else
                    {
                        listKey = new List<Guid>();
                        HttpContext.Current.Items.Add("PerRequestLifetimeManager",listKey);
                    }

                    listKey.Add(key);
                }
            }
        }

        public static void Dispose()
        {
            if (HttpContext.Current.Items.Contains("PerRequestLifetimeManager"))
            {
                List<Guid> listKey = (List<Guid>)HttpContext.Current.Items["PerRequestLifetimeManager"];
                foreach (Guid key in listKey)
                {
                    var item = HttpContext.Current.Items[key];
                    var dis = item as IDisposable;
                    if (dis != null)
                        dis.Dispose();
                }
            }
        }
    }
}