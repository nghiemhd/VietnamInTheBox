using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.BusinessObjects;

namespace Zit.BusinessLogic
{
    public interface IStoreBusiness
    {
        List<CF_Store> GetAllStore();
    }
}
