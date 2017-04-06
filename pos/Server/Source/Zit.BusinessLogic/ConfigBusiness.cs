using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Zit.BusinessObjects.BusinessModels;
using Zit.Configurations;
using Zit.Core;

namespace Zit.BusinessLogic
{
    public class ConfigBusiness : BusinessBase, IConfigBusiness
    {
        private static AppConfigClient _appConfig = null;

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public AppConfigClient GetAppConfig()
        {
            if (_appConfig == null)
            {
                _appConfig = Mapper.Map<AppConfigClient>(AppConfig.Current);
            }
            return _appConfig;
        }
    }
}
