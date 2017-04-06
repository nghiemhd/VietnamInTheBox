using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public class SysConfigAppRepository : EFRepository<SYS_ConfigApp>, ISysConfigAppRepository
    {
        public SysConfigAppRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public IEnumerable<SYS_ConfigApp> GetAll()
        {
            return this.RepositoryQuery.AsEnumerable();
        }

        public SYS_ConfigApp GetConfig(string key)
        {
            return this.RepositoryQuery.Where(m => m.ConfigKey == key).FirstOrDefault();
        }
    }
}
