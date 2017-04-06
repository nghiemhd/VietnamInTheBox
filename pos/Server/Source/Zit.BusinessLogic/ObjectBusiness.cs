using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.BusinessObjects;
using Zit.DataObjects;
using Zit.EntLib.Extensions;

namespace Zit.BusinessLogic
{
    public class ObjectBusiness : IObjectBusiness
    {
        public List<CF_Obj> GetAllObj()
        {
            var rp = IoC.Get<IObjectRepository>();
            return rp.GetAll();
        }

        public List<CF_Carrier> GetAllCarrier()
        {
            var rp = IoC.Get<ICarrierRepository>();
            return rp.GetAll();
        }
    }
}
