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
    public class CommonBusiness : ICommonBusiness
    {
        public List<CF_SaleReturnReason> GetAllSaleReturnReason()
        {
            var rp = IoC.Get<ISaleReturnReasonRepository>();
            return rp.GetAll();
        }

        public List<CF_SaleSource> GetAllSaleSource()
        {
            var rp = IoC.Get<ISaleSourceRepository>();
            return rp.GetAll();
        }
    }
}
