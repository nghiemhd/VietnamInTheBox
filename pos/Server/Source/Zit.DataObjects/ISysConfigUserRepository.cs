using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.Core.Repository;
using Zit.BusinessObjects;

namespace Zit.DataObjects
{
    public interface ISysConfigUserRepository : IRepository<SYS_ConfigUser>
    {
        IEnumerable<SYS_ConfigUser> GetConfigByUserName(string userName);
        SYS_ConfigUser GetConfig(string userName, string key);
    }
}
