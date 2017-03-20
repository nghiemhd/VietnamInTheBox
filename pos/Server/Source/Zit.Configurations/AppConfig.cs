using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.Practices.ServiceLocation;
using Zit.Core.Repository;
using Zit.BusinessObjects;
using Zit.DataObjects;

namespace Zit.Configurations
{
    public class AppConfig
    {
        #region Private

        private NameValueCollection _readOnlyConfig;
        private readonly Lazy<Dictionary<string, string>> _lazydataConfig;
        private Dictionary<string, string> _dataConfig
        {
            get
            {
                return _lazydataConfig.Value;
            }
        }
        private static AppConfig _current = null;

        private string getDataStringValue(string key, string defaultValue)
        {
            if (_dataConfig.ContainsKey(key))
                return _dataConfig[key];
            else
                return defaultValue;
        }

        private void setDataStringValue(string key, string value, string groupCode)
        {
            ISysConfigAppRepository _configAppRp = ServiceLocator.Current.GetInstance<ISysConfigAppRepository>();
            IUnitOfWork _unitOfWork = ServiceLocator.Current.GetInstance<IUnitOfWork>();
            //Check Exist
            var current = _configAppRp.GetConfig(key);
            if (current == null)
            {
                SYS_ConfigApp config = new SYS_ConfigApp()
                {
                    ConfigKey = key,
                    Val = value,
                    GroupCode = groupCode
                };
                _configAppRp.Add(config);
            }
            else
            {
                current.Val = value;
                _configAppRp.Update(current);
            }
            _unitOfWork.Commit();
            //
            if (_dataConfig.ContainsKey(key))
            {
                _dataConfig[key] = value;
            }
            else
            {
                _dataConfig.Add(key, value);
            }
        }

        #endregion

        #region Constructors

        public AppConfig()
        {
            _readOnlyConfig = ConfigurationManager.AppSettings;
            _lazydataConfig = new Lazy<Dictionary<string, string>>(() => {
                ISysConfigAppRepository _configAppRp = ServiceLocator.Current.GetInstance<ISysConfigAppRepository>();
                return _configAppRp.GetAll().ToDictionary(m => m.ConfigKey, m => m.Val);
            });
        }

        public static AppConfig Current
        {
            get
            {
                if (_current == null) _current = new AppConfig();
                return _current;
            }
        }

        #endregion

        #region Configures

        #region Security

        public string HashSalt
        {
            get
            {
                return _readOnlyConfig[ConfigKey.HASHSALT];
            }
        }

        #endregion

        #region Others

        #endregion

        #endregion
    }
}
