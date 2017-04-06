using System;
using System.Collections.Generic;
using System.Web;
using System.ServiceModel.Activation;
using Microsoft.Practices.ServiceLocation;
using System.ServiceModel.Web;

namespace Zit.Wcf.Libs
{
    public class ZitWebServiceHostFactory : WebServiceHostFactory
    {
        protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ZitWebServiceHost webHost = new ZitWebServiceHost(serviceType, baseAddresses);
            return webHost;
        }
    }
}