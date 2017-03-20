using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;

namespace Zit.Wcf.Libs
{
    public class TokenClientMessageInspector : IClientMessageInspector
    {
        private static Dictionary<string, string> _tokens = new Dictionary<string, string>();
        private string tokenName = null;

        public TokenClientMessageInspector(string tokenName)
        {
            this.tokenName = tokenName;
        }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            if (reply.Headers.FindHeader(ZitTokenContainer.TOKENNAME, ZitTokenContainer.TOKENNAMESPACE) >= 0)
            {
                var token = reply.Headers.GetHeader<string>(ZitTokenContainer.TOKENNAME, ZitTokenContainer.TOKENNAMESPACE);
                if (token != null)
                {
                    lock (_tokens)
                    {
                        if (_tokens.ContainsKey(tokenName))
                        {
                            _tokens[tokenName] = token;
                        }
                        else
                        {
                            _tokens.Add(tokenName, token);
                        }
                    }
                }
            }
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            string token = null;
            lock (_tokens)
            {
                if (_tokens.ContainsKey(tokenName))
                    token = _tokens[tokenName];
            }
            if (token != null)
            {
                int index = request.Headers.FindHeader(ZitTokenContainer.TOKENNAME, ZitTokenContainer.TOKENNAMESPACE);
                if (index >= 0)
                {
                    request.Headers.RemoveAt(index);
                }
                request.Headers.Add(MessageHeader.CreateHeader(ZitTokenContainer.TOKENNAME, ZitTokenContainer.TOKENNAMESPACE, token));
            }
            return null;
        }
    }
}
