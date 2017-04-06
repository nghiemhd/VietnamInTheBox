using System;
using System.Collections.Generic;
using System.Web;
using System.ServiceModel;

namespace Zit.Wcf.Libs
{
    public class ZitServiceHost : ServiceHost
    {
        public ZitServiceHost(Type serviceType, params Uri[] baseAddresses)
        : base(serviceType, baseAddresses)
        { }

        protected override void OnOpen(TimeSpan timeout)
        {
            Description.Behaviors.Add(new ZitServiceBehavior());
            base.OnOpen(timeout);
        }
    }


}