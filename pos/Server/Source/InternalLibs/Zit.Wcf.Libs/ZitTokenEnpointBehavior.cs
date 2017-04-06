using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Description;

namespace Zit.Wcf.Libs
{
    public class ZitTokenEnpointBehavior : IEndpointBehavior
    {
        private string tokenName = null;
        public ZitTokenEnpointBehavior(string tokenName)
        {
            this.tokenName = tokenName;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new TokenClientMessageInspector(tokenName));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
