using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;

namespace Zit.DataObjects.Views
{
    public interface IViewRoleMenuRepository
    {
        IEnumerable<vw_RoleMenu> GetByRoles(int[] roles);
    }
}
