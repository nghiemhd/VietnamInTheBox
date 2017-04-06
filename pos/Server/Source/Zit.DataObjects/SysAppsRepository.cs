using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.Core.Repository;
using Zit.BusinessObjects;

namespace Zit.DataObjects
{
    public class SysAppsRepository :EFRepository<SYS_Apps>,ISysAppsRepository
    {
        public SysAppsRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public SYS_Apps GetByKey(string key)
        {
            return base.RepositoryQuery.Where(m => m.AppKey == key).FirstOrDefault();
        }
    }
}
