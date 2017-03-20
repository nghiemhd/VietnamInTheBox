using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;

namespace Zit.Wcf.Libs
{
    public class PerRequestLifetimeManager : LifetimeManager
    {
        private Guid key;

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
            if (WcfOperationContext.Current == null)
            {
                return null;
            }
            else
            {
                return WcfOperationContext.Current.Items[key];
            }
        }

        /// <summary>
        /// Removes the value.
        /// </summary>
        public override void RemoveValue()
        {
            if (WcfOperationContext.Current != null)
            {
                WcfOperationContext.Current.Items.Remove(key);
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        public override void SetValue(object newValue)
        {
            if (WcfOperationContext.Current != null)
            {
                if (WcfOperationContext.Current.Items.Contains(key))
                    WcfOperationContext.Current.Items[key] = newValue;
                else WcfOperationContext.Current.Items.Add(key, newValue);
            }
        }
    }
}
