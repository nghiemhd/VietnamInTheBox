using System;
using System.Collections.Generic;
using System.Web;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using log4net;

namespace Zit.Wcf.Libs
{
    public class ZitServiceBehavior : IServiceBehavior
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            foreach (var cdb in serviceHostBase.ChannelDispatchers)
            {
                var cd = cdb as ChannelDispatcher;

                if (cd != null)
                {
                    foreach (var ed in cd.Endpoints)
                    {
                        //Instant Using Unity
                        ed.DispatchRuntime.InstanceProvider = new ZitInstanceProvider(serviceDescription.ServiceType);
                        //Add Token Dispatch
                        ed.DispatchRuntime.MessageInspectors.Add(new TokenDispatchMessageInspector());
                        //
                    }

                    //add Error Handler
                    bool isExist = false;
                    foreach (IErrorHandler errHandler in cd.ErrorHandlers)
                    {
                        if (errHandler is ZitServiceBehavior)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        cd.ErrorHandlers.Add(new ZitLogHandler());
                    }
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {

        }
    }
}