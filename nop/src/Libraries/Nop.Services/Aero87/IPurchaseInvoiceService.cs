using Nop.Core.Domain.Aero87;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Aero87
{
    public interface IPurchaseInvoiceService
    {
        IList<PurchaseInvoicesResult> Search(
            DateTime? fromDate,
            DateTime? toDate,
            int? storeId,
            int? status,
            string invoiceNo,
            string refNo,
            string productCode,
            string barcode,
            int pageIndex, int pageSize,
            out int totalRecord,
            out decimal sumAmount,
            out int sumQty
            );
        IList<PurchaseInvoiceDetailLinesResult> GetPurchaseInvoiceDetail(
            int invoiceId,
            int pageIndex, int pageSize,
            out int totalRecord);

        void PurchaseInvoiceApprove(
            int invoiceId,
            int creditAcctId,
            int? objId,
            string desc,
            string user
            );
        void PurchaseInvoiceUnPost(
            int invoiceId,
            string user
            );
    }
}
