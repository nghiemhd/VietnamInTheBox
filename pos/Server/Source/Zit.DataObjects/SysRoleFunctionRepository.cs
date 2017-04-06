using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public class SysRoleFunctionRepository : EFRepository<SYS_RoleFunction>, ISysRoleFunctionRepository
    {

        public SysRoleFunctionRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public IEnumerable<SYS_RoleFunction> GetByRoles(int[] roles)
        {
            return base.RepositoryQuery.Where(m => roles.Contains(m.RoleID) && m.SYS_Function.Enable && m.SYS_Role.Enable);
        }
    }
}
