using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.BusinessObjects.BusinessModels;

namespace Zit.BusinessLogic
{
    public interface IConfigBusiness
    {
        AppConfigClient GetAppConfig();
    }
}
