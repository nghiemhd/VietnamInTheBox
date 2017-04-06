using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace Zit.EntLib.Extensions
{
    public static class UnityServiceLocatorExtensions
    {
        public static T GetInstance<T>(this IServiceLocator locator, ParameterOverride para)
        {
            IUnityContainer container = locator.GetInstance<IUnityContainer>();
            return container.Resolve<T>(para);
        }
        public static T GetInstance<T>(this IServiceLocator locator, string name, ParameterOverride para)
        {
            IUnityContainer container = locator.GetInstance<IUnityContainer>();
            return container.Resolve<T>(name, para);
        }

    }
}
