using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;

namespace Zit.Security
{
    public class ZitSession : IDisposable
    {
        const string PRINCIPAL = "PRINCIPAL";
        const string CULTURE = "CULTURE";
        const string TIMEZONE = "TIMEZONE";
        const string APPTYPE = "APPTYPE";
        Dictionary<string, object> _store = new Dictionary<string,object>();
        string token = null;
        ISessionContainer sessionCtn = null;
        ITokenContainer tokenContainer = null;

        public object this[string key]
        {
            get
            {
                if (_store.ContainsKey(key))
                    return _store[key];
                else return null;
            }
            set
            {
                if (_store.ContainsKey(key)) _store.Remove(key);
                _store.Add(key, value);
                __updateContainer();
            }
        }


        public ZitSession(string token)
        {
            this.token = token;
            sessionCtn = ServiceLocator.Current.GetInstance<ISessionContainer>();
            tokenContainer = ServiceLocator.Current.GetInstance<ITokenContainer>();
            sessionCtn.SetSession(token, this);
        }

        public string Token
        {
            get
            {
                return token;
            }
        }        

        public IZitPrincipal Principal
        {
            get {
                return this[PRINCIPAL] as IZitPrincipal;
            }
            set
            {
                this[PRINCIPAL] = value;
            }
        }        

        public string Culture
        {
            get {
                return this[CULTURE] as string;
            }
            set
            {
                this[CULTURE] = value;                
            }
        }

        public AppTypeEnum AppType {
            get
            {
                return (AppTypeEnum)(this[APPTYPE]??0);
            }
            set
            {
                this[APPTYPE] = value;
            }
        }

        public int TimeZone
        {
            get
            {
                return (int)(this[TIMEZONE]??0);
            }
            set
            {
                this[TIMEZONE] = value;                
            }
        }


        private void __updateContainer()
        {
            sessionCtn.SetSession(Token, this);
        }

        private static IRequestContainer _requestContainer = null;
        private static IRequestContainer RequestContainer
        {
            get
            {
                if(_requestContainer == null)
                    _requestContainer = ServiceLocator.Current.GetInstance<IRequestContainer>();
                return _requestContainer;
            }
        }

        public static ZitSession Current {
            get
            {
                var sessionCtn = ServiceLocator.Current.GetInstance<ISessionContainer>();
                var tokenCtn = ServiceLocator.Current.GetInstance<ITokenContainer>();
                var token = tokenCtn.GetToken();
                if (token == null)
                {
                    token = tokenCtn.Issue();
                    tokenCtn.SetToken(token);
                    ZitSession session = new ZitSession(token);
                    return session;
                }
                else
                {

                    if (RequestContainer.Contain(token))
                    {
                        return RequestContainer.Get<ZitSession>(token);
                    }
                    else
                    {
                        var curr = sessionCtn.GetSession(token);
                        if (curr == null)
                        {
                            token = tokenCtn.Issue();
                            tokenCtn.SetToken(token);
                            curr = new ZitSession(token);
                        }

                        RequestContainer.Set(token, curr);

                        return curr;
                    }
                }
            }
            set
            {
                if (value == null) throw new ArgumentNullException("Session");
                var setSession = value;
                var tokenCtn = ServiceLocator.Current.GetInstance<ITokenContainer>();
                //Update Token Container
                tokenCtn.SetToken(setSession.Token);
                var sessionCtn = ServiceLocator.Current.GetInstance<ISessionContainer>();
                sessionCtn.SetSession(setSession.Token, setSession);

                RequestContainer.Set(setSession.Token, setSession);
            }
        }

        public void SetExpireAll()
        {
            if (IsAuthenticated)
            {
                var sessionCtn = ServiceLocator.Current.GetInstance<ISessionContainer>();
                sessionCtn.ExpireUser(this.Principal.Identity.Name);
            }
        }

        public static void SetExpireAll(string userName)
        {
            var sessionCtn = ServiceLocator.Current.GetInstance<ISessionContainer>();
            sessionCtn.ExpireUser(userName);
        }

        public DateTime CreatedDate = DateTime.Now;

        /// <summary>
        /// Try Get Session From Container
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static ZitSession GetSession(string token)
        {
            var sessionCtn = ServiceLocator.Current.GetInstance<ISessionContainer>();
            return sessionCtn.GetSession(token);
        }

        private bool disposing = false;

        public void Dispose()
        {
            if (!disposing)
            {
                disposing = true;
                sessionCtn.Remove(Token);
            }
        }

        public bool IsAuthenticated {
            get
            {
                if (this.Principal != null && this.Principal.Identity != null)
                {
                    return this.Principal.Identity.IsAuthenticated;
                }
                else return false;
            }
        }
    }

    [Flags]
    public enum AppTypeEnum : int
    {
        Web = 1,

        ServiceBus = 2,

        SenPay = 3,

        Magento = 4,

        Carrier = 5,

        Email = 7,

        Inside = 8
    }
}
