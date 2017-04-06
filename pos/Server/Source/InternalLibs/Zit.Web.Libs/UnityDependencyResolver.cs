using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Web.Mvc;

namespace Zit.Web.Libs
{
    public class UnityDependencyResolver : IDependencyResolver
    {
        private readonly IUnityContainer _container;

        public UnityDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            if (!typeof(IController).IsAssignableFrom(serviceType))
                return null;
            return _container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.ResolveAll(serviceType);
        }
    }
}
