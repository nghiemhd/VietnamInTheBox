using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public class SysAppFunctionRepository : EFRepository<SYS_AppFunction>, ISysAppFunctionRepository
    {
        public SysAppFunctionRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public IEnumerable<SYS_AppFunction> GetByAppID(int appID)
        {
            return base.RepositoryQuery.Where(m => m.AppID == appID).AsEnumerable();
        }
    }
}
