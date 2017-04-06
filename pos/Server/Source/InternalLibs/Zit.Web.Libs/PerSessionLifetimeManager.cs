using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Web;

namespace Zit.Web.Libs
{
    public class PerSessionLifetimeManager : LifetimeManager
    {
        private readonly string key;

        public PerSessionLifetimeManager()
        {
            key = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Session[key];
            }
        }

        /// <summary>
        /// Removes the value.
        /// </summary>
        public override void RemoveValue()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Remove(key);
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        public override void SetValue(object newValue)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[key] = newValue;
            }
        }
    }
}
