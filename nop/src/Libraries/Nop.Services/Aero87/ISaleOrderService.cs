using Nop.Core.Domain.Aero87;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Aero87
{
    public interface ISaleOrderService
    {
        IList<SaleOrdersResult> Search(
            SalesOrderSearchDTO searchCondition,
            int pageIndex, int pageSize,
            out int totalRecord,
            out decimal sumSubTotal,
            out decimal sumDiscount,
            out decimal sumShippingFee,
            out decimal sumAeroShippingFee,
            out decimal sumAmount,
            out decimal sumCostAmount,
            out int sumQty
            );
        IList<SaleOrderDetailLinesResult> GetSaleOrderDetail(int orderId);

        void SaleOrderApprove(
            int orderId,
            int debitAcctId,
            int? objId,
            int? feeAcctId,
            decimal? feeAmount,
            string desc,
            string user
            );
        void SaleOrderUnPost(
            int orderId,
            string user
            );

        POS_SaleOrder GetSaleOrderById(int orderId);

        void UpdateSaleOrder(UpdatedSaleOrderDTO order);
    }
}
