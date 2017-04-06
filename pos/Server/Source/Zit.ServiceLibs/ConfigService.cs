using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.BusinessLogic;
using Zit.BusinessObjects.BusinessModels;
using Zit.EntLib.Extensions;

namespace Zit.ServiceLibs
{
    public partial class ZitServices : IConfigService
    {
        public AppConfigClient GetAppConfig()
        {
            return IoC.Get<IConfigBusiness>().GetAppConfig();
        }
    }
}
