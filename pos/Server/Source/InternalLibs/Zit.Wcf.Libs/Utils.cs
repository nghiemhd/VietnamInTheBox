using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Zit.Wcf.Libs
{
    public static class Utils
    {
        public static string GetRequestIpAddress(this OperationContext ctx)
        {
            MessageProperties prop = ctx.IncomingMessageProperties;
            RemoteEndpointMessageProperty enpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return (enpoint == null)?"":enpoint.Address;
        }
    }
}
