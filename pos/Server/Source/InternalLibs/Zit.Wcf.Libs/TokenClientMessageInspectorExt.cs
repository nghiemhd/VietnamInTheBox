using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;

namespace Zit.Wcf.Libs
{
    public class TokenClientMessageInspectorExt : IClientMessageInspector
    {
        private object lockObj = new object();
        private string _token;

        public TokenClientMessageInspectorExt(string token)
        {
            this._token = token;
        }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            if (reply.Headers.FindHeader(ZitTokenContainer.TOKENNAME, ZitTokenContainer.TOKENNAMESPACE) >= 0)
            {
                var token = reply.Headers.GetHeader<string>(ZitTokenContainer.TOKENNAME, ZitTokenContainer.TOKENNAMESPACE);
                if (token != null)
                {
                    lock (lockObj)
                    {
                        _token = token;
                    }
                }
            }
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            if (_token != null)
            {
                int index = request.Headers.FindHeader(ZitTokenContainer.TOKENNAME, ZitTokenContainer.TOKENNAMESPACE);
                if (index >= 0)
                {
                    request.Headers.RemoveAt(index);
                }
                request.Headers.Add(MessageHeader.CreateHeader(ZitTokenContainer.TOKENNAME, ZitTokenContainer.TOKENNAMESPACE, _token));
            }
            return null;
        }
    }
}
