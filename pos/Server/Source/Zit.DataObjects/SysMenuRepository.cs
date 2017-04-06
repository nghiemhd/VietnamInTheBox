using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;
using Zit.Core.Repository;
using Zit.Core;

namespace Zit.DataObjects
{
    public class SysMenuRepository : EFRepository<SYS_Menu>, ISysMenuRepository
    {
        public SysMenuRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public IEnumerable<SYS_Menu> GetAll()
        {
            return base.RepositoryQuery.AsEnumerable();
        }
    }
}
