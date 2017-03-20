using System.Collections.Generic;

using Zit.BusinessObjects;
using Zit.Core;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public interface ISysUserRepository : IRepository<SYS_User>
    {
        SYS_User GetUserByUserName(string userName, string password);
        SYS_User GetUserByUserName(string userName);
        IEnumerable<SYS_User> GetUsersByRole(int roleID);
    }
}