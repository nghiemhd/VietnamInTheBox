using System;
using System.Collections.Generic;
using System.Text;
using Zit.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Net;
using System.Xml;
using System.Text.RegularExpressions;

namespace Zit.Wcf.Libs
{
    public class ZitTokenContainer : ITokenContainer
    {
        public const string TOKENNAME = "TOKEN";
        public const string TOKENNAMESPACE = "SENDO";

        public string GetToken()
        {
            string tokenid = null;
            if (OperationContext.Current.IncomingMessageVersion == MessageVersion.None)
            {
                var rrr = OperationContext.Current.RequestContext.RequestMessage.ToString();
                #region REST
                if (OperationContext.Current.RequestContext.RequestMessage.State != MessageState.Closed && OperationContext.Current.IncomingMessageProperties.ContainsKey(HttpRequestMessageProperty.Name))
                {
                    HttpRequestMessageProperty requestProperty = OperationContext.Current.IncomingMessageProperties[HttpRequestMessageProperty.Name]
                                                                    as HttpRequestMessageProperty;
                    if (requestProperty != null)
                    {
                        tokenid = __parseFromCookie(requestProperty.Headers[HttpRequestHeader.Cookie]);
                    }
                }

                //Try Get From Response
                if (OperationContext.Current.RequestContext.RequestMessage.State != MessageState.Closed && tokenid == null && OperationContext.Current.OutgoingMessageProperties.ContainsKey(HttpResponseMessageProperty.Name))
                {
                    var response = (OperationContext.Current.OutgoingMessageProperties[HttpResponseMessageProperty.Name] as HttpResponseMessageProperty);
                    if (response != null)
                    {
                        tokenid = __parseFromCookie(response.Headers[HttpResponseHeader.SetCookie]);
                    }
                }
                #endregion
            }
            else
            {
                #region SOAP
                //Get From Request
                int index = -1;
                if ((index = OperationContext.Current.IncomingMessageHeaders.FindHeader(TOKENNAME, TOKENNAMESPACE)) >= 0)
                {
                    tokenid = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>(TOKENNAME, TOKENNAMESPACE);
                }
                //Try Get From Response
                if (tokenid == null && (index = OperationContext.Current.OutgoingMessageHeaders.FindHeader(TOKENNAME, TOKENNAMESPACE)) >= 0)
                {
                    tokenid = OperationContext.Current.OutgoingMessageHeaders.GetHeader<string>(TOKENNAME, TOKENNAMESPACE);                    
                }
                #endregion
            }
            return tokenid;
        }

        public void SetToken(string token)
        {
            if (OperationContext.Current.IncomingMessageVersion == MessageVersion.None)
            {
                #region REST
                //Set For Response
                HttpResponseMessageProperty property;
                if (OperationContext.Current.OutgoingMessageProperties.ContainsKey(HttpResponseMessageProperty.Name))
                {
                    property = (OperationContext.Current.OutgoingMessageProperties[HttpResponseMessageProperty.Name] as HttpResponseMessageProperty);
                    property.Headers.Remove(HttpResponseHeader.SetCookie);
                    property.Headers.Add(HttpResponseHeader.SetCookie, FormatForCookie(token));
                    
                }
                else
                {
                    property = new HttpResponseMessageProperty();
                    property.Headers.Add(HttpResponseHeader.SetCookie, FormatForCookie(token));
                    OperationContext.Current.OutgoingMessageProperties.Add(HttpResponseMessageProperty.Name, property);
                }
                //Set For Request
                HttpRequestMessageProperty requestProperty;
                if (OperationContext.Current.IncomingMessageProperties.ContainsKey(HttpRequestMessageProperty.Name))
                {
                    requestProperty = (OperationContext.Current.IncomingMessageProperties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty);
                    requestProperty.Headers.Remove(HttpRequestHeader.Cookie);
                    requestProperty.Headers.Add(HttpRequestHeader.Cookie, FormatForCookie(token));

                }
                else
                {
                    requestProperty = new HttpRequestMessageProperty();
                    requestProperty.Headers.Add(HttpRequestHeader.Cookie, FormatForCookie(token));
                    OperationContext.Current.IncomingMessageProperties.Add(HttpRequestMessageProperty.Name, property);
                }
                #endregion
            }
            else
            {
                #region SOAP
                int index = -1;
                //Remove
                if ((index = OperationContext.Current.OutgoingMessageHeaders.FindHeader(TOKENNAME, TOKENNAMESPACE)) >= 0)
                {
                    OperationContext.Current.OutgoingMessageHeaders.RemoveAt(index);
                }
                if ((index = OperationContext.Current.IncomingMessageHeaders.FindHeader(TOKENNAME, TOKENNAMESPACE)) >= 0)
                {
                    OperationContext.Current.IncomingMessageHeaders.RemoveAt(index);
                }
                //Add New
                MessageHeader msgHeader = MessageHeader.CreateHeader(TOKENNAME, TOKENNAMESPACE, token);
                OperationContext.Current.IncomingMessageHeaders.Add(msgHeader);
                OperationContext.Current.OutgoingMessageHeaders.Add(msgHeader);
                #endregion
            }
        }

        public string Issue()
        {
            return Guid.NewGuid().ToString().Replace("-","");
        }

        public static string FormatForCookie(string token)
        {
            return TOKENNAME + "=" + token;
        }

        private static string __parseFromCookie(string cookies)
        {
            if (cookies == null) return null;
            foreach (var cookie in cookies.Split(';'))
            {
                string[] values = cookie.Split('=');
                if (values != null && values.Length == 2 && values[0].Trim() == TOKENNAME)
                {
                    return values[1];
                }
            }
            return null;
        }

        //private static string __parseFromHeader(string header)
        //{
        //    if (header == null) return null;
        //    Regex reg = new Regex(@"<TOKEN.*\>(.*)<\/TOKEN>");
        //    return reg.Match(header).Groups[1].Value;
        //}
    }
}
