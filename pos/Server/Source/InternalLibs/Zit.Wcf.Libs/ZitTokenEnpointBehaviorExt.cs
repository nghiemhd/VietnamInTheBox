using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Description;

namespace Zit.Wcf.Libs
{
    public class ZitTokenEnpointBehaviorExt : IEndpointBehavior
    {
        private string token = null;
        public ZitTokenEnpointBehaviorExt(string token)
        {
            this.token = token;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new TokenClientMessageInspectorExt(token));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
