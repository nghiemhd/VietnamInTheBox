using System;
using System.Collections.Generic;
using System.Web;
using System.ServiceModel.Activation;
using Microsoft.Practices.ServiceLocation;
using System.ServiceModel;

namespace Zit.Wcf.Libs
{
    public class ZitServiceHostFactory : ServiceHostFactory
    {
        protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost serviceHost = new ZitServiceHost(serviceType, baseAddresses);
            return serviceHost;
        }
    }


}