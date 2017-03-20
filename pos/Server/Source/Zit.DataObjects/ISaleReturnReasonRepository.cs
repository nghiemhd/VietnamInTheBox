using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public interface ISaleReturnReasonRepository : IRepository<CF_SaleReturnReason>
    {
        List<CF_SaleReturnReason> GetAll();
    }
}
