using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public interface ISysRoleFunctionRepository : IRepository<SYS_RoleFunction>
    {
        IEnumerable<SYS_RoleFunction> GetByRoles(int[] roles);
    }
}
