using System;
using System.Collections.Generic;
using System.Web;
using System.ServiceModel.Web;

namespace Zit.Wcf.Libs
{
    public class ZitWebServiceHost : WebServiceHost
    {
        public ZitWebServiceHost(Type serviceType, Uri[] baseAddresses)
        : base(serviceType, baseAddresses)
        { 
            
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            Description.Behaviors.Add(new ZitServiceBehavior());
            base.OnOpen(timeout);
        }
    }
}