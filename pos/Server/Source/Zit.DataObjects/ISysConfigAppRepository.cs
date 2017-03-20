using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.Core.Repository;
using Zit.BusinessObjects;

namespace Zit.DataObjects
{
    public interface ISysConfigAppRepository : IRepository<SYS_ConfigApp>
    {
        SYS_ConfigApp GetConfig(string key);
        IEnumerable<SYS_ConfigApp> GetAll();
    }
}
