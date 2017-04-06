using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.BusinessObjects.BusinessModels;
using Zit.Core;

namespace Zit.BusinessLogic
{
    public interface IHrBusiness : IBusiness
    {
        void UserCheckTime(string userName, string password);
        string GetUserByName(string userName);
        void UpdateUser(string userName, string fullName, string password, bool isActive);
    }
}
