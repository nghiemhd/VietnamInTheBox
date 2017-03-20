using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;
using Zit.Core.Repository;

namespace Zit.DataObjects.Views
{
    public class ViewRoleMenuRepository : EFRepository<vw_RoleMenu>, IViewRoleMenuRepository
    {
        public ViewRoleMenuRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public IEnumerable<vw_RoleMenu> GetByRoles(int[] roles)
        {
            return base.RepositoryQuery.Where(m => roles.Contains(m.RoleID)).AsEnumerable();
        }
    }
}
