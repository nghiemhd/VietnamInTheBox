using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;

namespace Zit.DataObjects
{
    public interface ISysAppFunctionRepository
    {
        IEnumerable<SYS_AppFunction> GetByAppID(int appID);
    }
}
