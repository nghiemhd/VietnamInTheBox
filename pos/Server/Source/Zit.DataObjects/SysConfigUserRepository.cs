using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.Core.Repository;
using Zit.BusinessObjects;

namespace Zit.DataObjects
{
    public class SysConfigUserRepository : EFRepository<SYS_ConfigUser>, ISysConfigUserRepository
    {
        public SysConfigUserRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public IEnumerable<SYS_ConfigUser> GetConfigByUserName(string userName)
        {
            return this.RepositoryQuery.Where(m => m.UserName == userName);
        }


        public SYS_ConfigUser GetConfig(string userName, string key)
        {
            return this.RepositoryQuery.Where(m => m.UserName == userName && m.ConfigKey == key).FirstOrDefault();
        }
    }
}
