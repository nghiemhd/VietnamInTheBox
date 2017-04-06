using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.BusinessObjects;
using Zit.Security;

namespace Sendo.BusinessLogic
{
    public static class WorkContext
    {
        const string WCT = "WCF";

        public static UserContext UserContext 
        {
            get
            {
                return ZitSession.Current[WCT] as UserContext;
            }
            set
            {
                ZitSession.Current[WCT] = value;
            }
        }
    }
}
