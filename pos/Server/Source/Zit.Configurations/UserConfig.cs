using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Zit.Security;
using Zit.BusinessObjects;
using Zit.DataObjects;
using Zit.Core.Repository;

namespace Zit.Configurations
{
    public class UserConfig
    {
        #region Private

        Dictionary<string,string> _config = null;
        string userName = null;

        private string getStringValue(string key, string defaultValue)
        {
            if (_config.ContainsKey(key))
                return _config[key];
            else
                return defaultValue;
        }

        private void setStringValue(string key, string value, string groupCode)
        {
            ISysConfigUserRepository _configUserRp = ServiceLocator.Current.GetInstance<ISysConfigUserRepository>();
            IUnitOfWork _unitOfWork = ServiceLocator.Current.GetInstance<IUnitOfWork>();
            //Check Exist
            var current = _configUserRp.GetConfig(userName, key);
            if (current == null)
            {
                SYS_ConfigUser config = new SYS_ConfigUser()
                {
                    UserName = userName,
                    ConfigKey = key,
                    Val = value,
                    GroupCode = groupCode
                };
                _configUserRp.Add(config);
            }
            else
            {
                current.Val = value;
                _configUserRp.Update(current);
            }
            _unitOfWork.Commit();
            //Save To Dic
            if (_config.ContainsKey(key))
            {
                _config[key] = value;
            }
            else
            {
                _config.Add(key, value);
            }
        }

        #endregion

        #region Constructor

        public static UserConfig Current
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UserConfig>();
            }
        }

        public UserConfig()
        {
            //Read User Config
            //Get From Database Where UserName is ZitSession
            if(
                ZitSession.Current.Principal == null 
                || ZitSession.Current.Principal.Identity == null
                || (!ZitSession.Current.Principal.Identity.IsAuthenticated)
                )
                throw new InvalidOperationException("Can't load user config from here");
            ISysConfigUserRepository _configUserRp = ServiceLocator.Current.GetInstance<ISysConfigUserRepository>();
            userName = ZitSession.Current.Principal.Identity.Name;
            _config = _configUserRp.GetConfigByUserName(userName).ToDictionary(m => m.ConfigKey, m => m.Val);
        }
        #endregion

        #region Configures

        #endregion
    }
}
