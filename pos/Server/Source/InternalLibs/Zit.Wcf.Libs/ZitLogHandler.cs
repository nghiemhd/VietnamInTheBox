using System;
using System.Collections.Generic;
using System.Web;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using log4net;

namespace Zit.Wcf.Libs
{
    public class ZitLogHandler : IErrorHandler
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(ZitLogHandler));

        public bool HandleError(Exception error)
        {
            _log.Error(error);
            return false;
        }

        public void ProvideFault(Exception error, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
        }
    }
}