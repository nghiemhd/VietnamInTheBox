using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.BusinessObjects;

namespace Zit.BusinessLogic
{
    public interface IObjectBusiness
    {
        List<CF_Obj> GetAllObj();
        List<CF_Carrier> GetAllCarrier();
    }
}
