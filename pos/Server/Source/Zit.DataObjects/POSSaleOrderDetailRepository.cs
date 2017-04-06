using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.BusinessObjects;
using Zit.BusinessObjects.SqlResultModel;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public class POSSaleOrderDetailRepository : EFRepository<POS_SaleOrderDetail>, IPOSSaleOrderDetailRepository
    {
        public POSSaleOrderDetailRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public IEnumerable<usp_GetPOSSaleOrderDetailBySaleOrderId> GetBySaleOrderId(int saleOrderId)
        {
            var pSaleOrderId = new SqlParameter("saleOrderId", SqlDbType.Int) { Value = saleOrderId };

            return ExecQuery<usp_GetPOSSaleOrderDetailBySaleOrderId>("usp_GetPOSSaleOrderDetailBySaleOrderId @saleOrderId", pSaleOrderId);
        }

        public usp_CreatePOSDetailByBarcode CreatePOSDetailByBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode)) return null;

            var pBarcode = new SqlParameter("barcode", SqlDbType.VarChar) { Value = barcode };

            return ExecQuery<usp_CreatePOSDetailByBarcode>("usp_CreatePOSDetailByBarcode @barcode", pBarcode).FirstOrDefault();
        }
    }
}
