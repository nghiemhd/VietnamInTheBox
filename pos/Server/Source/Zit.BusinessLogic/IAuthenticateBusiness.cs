using System.Collections.Generic;
using Zit.BusinessObjects;
using Zit.Core;
using Zit.Security;

namespace Zit.BusinessLogic
{
    public interface IAuthenticateBusiness : IBusiness
    {
        UserContext Login(int storeId, string userName, string password, string appKey);
        bool VerifyPassword(string passwordHashed,string userName,string password);
        string HashPassword(string userName,string password);
        bool TryOverrideSecurityContextByToken(string token);
        SortedSet<ZitMenu> GetMenus();
        string[] GetRoles();
    }
}