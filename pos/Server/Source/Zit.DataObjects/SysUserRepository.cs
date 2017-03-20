using System;
using System.Collections.Generic;
using System.Linq;

using Zit.BusinessObjects;
using Zit.BusinessObjects.Enums;
using Zit.Core;
using Zit.Core.Repository;
using Zit.Utils;

namespace Zit.DataObjects
{
    public class SysUserRepository : EFRepository<SYS_User>, ISysUserRepository
    {
        public SysUserRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork) {}

        public SYS_User GetUserByUserName(string userName, string password)
        {
            return base.RepositoryQuery.FirstOrDefault(m => m.UserName == userName && m.Password == password && m.IsDeleted == false);
        }

        public SYS_User GetUserByUserName(string userName)
        {
            return base.RepositoryQuery.FirstOrDefault(m => m.UserName == userName && m.IsDeleted == false);
        }

        public IEnumerable<SYS_User> GetUsersByRole(int roleID)
        {
            return null;
        }
    }
}