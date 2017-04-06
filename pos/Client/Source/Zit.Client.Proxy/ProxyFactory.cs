using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.Wcf.Libs;
using Zit.Client.Proxy.ZitServices;

namespace Zit.Client.Proxy
{
    public static class ProxyFactory
    {
        public static IZitServices CreateZitServices(string token = null)
        {
            ZitServicesClient cl = new ZitServicesClient("Binding_IZitServices");
            cl.Endpoint.EndpointBehaviors.Add(new ZitTokenEnpointBehaviorExt(token));
            return cl;
        }

        public static IZitServices CreateZitServices_Release(string token = null)
        {
            ZitServicesClient cl = new ZitServicesClient("Binding_IZitServices_Release");
            cl.Endpoint.EndpointBehaviors.Add(new ZitTokenEnpointBehaviorExt(token));
            return cl;
        }
    }
}
