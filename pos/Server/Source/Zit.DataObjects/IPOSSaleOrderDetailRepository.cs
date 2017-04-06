using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.BusinessObjects;
using Zit.BusinessObjects.SqlResultModel;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public interface IPOSSaleOrderDetailRepository : IRepository<POS_SaleOrderDetail>
    {
        IEnumerable<usp_GetPOSSaleOrderDetailBySaleOrderId> GetBySaleOrderId(int saleOrderId);
        usp_CreatePOSDetailByBarcode CreatePOSDetailByBarcode(string barcode);
    }
}
