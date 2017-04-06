using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ServiceLocation;

namespace Zit.EntLib.Extensions
{
    public static class IoC
    {
        public static T Get<T>()
        {
            return ServiceLocator.Current.GetInstance<T>();
        }

        public static T Get<T>(string key)
        {
            return ServiceLocator.Current.GetInstance<T>(key);
        }
    }
}
