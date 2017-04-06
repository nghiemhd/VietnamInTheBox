using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;

namespace Zit.DataObjects
{
    public interface ISysAppsRepository
    {
        SYS_Apps GetByKey(string key);
        SYS_Apps GetByID(object id);
    }
}
