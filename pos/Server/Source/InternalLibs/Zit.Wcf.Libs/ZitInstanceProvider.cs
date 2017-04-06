using System;
using System.Collections.Generic;
using System.Web;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.Practices.ServiceLocation;

namespace Zit.Wcf.Libs
{
    public class ZitInstanceProvider : IInstanceProvider
    {
        private readonly Type _serviceType;

        public ZitInstanceProvider(Type serviceType)
        {
            _serviceType = serviceType;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext,null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return ServiceLocator.Current.GetInstance(_serviceType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            IDisposable disposable = instance as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}