using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;

using Zit.Core.Repository;
using Zit.BusinessObjects;

namespace Zit.DataObjects
{
    public class SysUserRoleRepository : EFRepository<SYS_UserRole>, ISysUserRoleRepository
    {
        public SysUserRoleRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public int[] GetRoles(string userName)
        {
            return base.RepositoryQuery.Where(m => m.UserName == userName).Select(m => m.RoleID).ToArray();
        }
    }
}
