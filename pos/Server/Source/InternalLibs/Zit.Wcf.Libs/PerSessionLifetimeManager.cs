using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;
using Zit.Security;

namespace Zit.Wcf.Libs
{
    public class PerSessionLifetimeManager : LifetimeManager
    {
        private string key;

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
            if (ZitSession.Current == null)
            {
                return null;
            }
            else
            {
                return ZitSession.Current[key];
            }
        }

        /// <summary>
        /// Removes the value.
        /// </summary>
        public override void RemoveValue()
        {
            if (ZitSession.Current != null)
            {
                ZitSession.Current[key] = null;
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        public override void SetValue(object newValue)
        {
            if (ZitSession.Current != null)
            {
                ZitSession.Current[key] = newValue;
            }
            else
                throw new InvalidOperationException("ZitSession is null ");
        }
    }
}
