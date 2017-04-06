using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public interface ISysUserRoleRepository : IRepository<SYS_UserRole>
    {
        int[] GetRoles(string userName);
    }
}
