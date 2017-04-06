using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using System.Net;
using Zit.Security;
using Microsoft.Practices.ServiceLocation;

namespace Zit.Wcf.Libs
{
    public class TokenDispatchMessageInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            if (reply != null && reply.Version == MessageVersion.None)
            {
                //For REST
                string token = ServiceLocator.Current.GetInstance<ITokenContainer>().GetToken();

                if (token == null) return;

                HttpResponseMessageProperty responseMsg = reply.Properties[HttpResponseMessageProperty.Name] as HttpResponseMessageProperty;

                if (responseMsg == null)
                {
                    responseMsg = new HttpResponseMessageProperty();
                    reply.Properties.Add(HttpResponseMessageProperty.Name, responseMsg);
                }
                responseMsg.Headers.Remove(HttpResponseHeader.SetCookie);
                responseMsg.Headers.Add(HttpResponseHeader.SetCookie,ZitTokenContainer.FormatForCookie(token));
            }
            else
            {
                //For SOAP
            }
        }
    }
}
