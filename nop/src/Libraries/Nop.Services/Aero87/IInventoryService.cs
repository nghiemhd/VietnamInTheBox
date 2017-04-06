using Nop.Core.Domain.Aero87;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Aero87
{
    public interface IInventoryService
    {
        IList<InventoryTransactionResult> GetInventoryTransaction(
            DateTime? fromDate,
            DateTime? toDate,
            string tranNo,
            string productCode,
            int? storeId,
            string barcode,
            int? typeId,
            int pageIndex,
            int pageSize,
            out int totalRecord,
            out int sumQty
            );

        void IVMTranITComplete(int transferId, int typeId, string user);

        IList<InventoryTransferLineResult> GetInventoryTransferDetail(int transferId);

        IList<InventoryTransferResult> GetInventoryTransfer(
            DateTime? fromDate,
            DateTime? toDate,
            string tranNo,
            string productCode,
            int? fromStoreId,
            int? toStoreId,
            string barcode,
            int? status,
            int pageIndex,
            int pageSize,
            out int totalRecord,
            out int sumQty
            );

        IList<WarehouseCountResult> WarehouseCount(
            int? storeId,
            DateTime toEndDate,
            IList<int> categoryIds,
            int manufacturerId,
            bool orderByAge,
            int? filterByQty,
            string productCode,
            int pageIndex,
            int pageSize,
            out int totalRecord,
            out int SumQty,
            out decimal SumCostAmount
            );
        IList<WarehouseCountLineResult> GetWarehouseCountDetail(int productId, int? storeId, DateTime toEndDate);

        IList<InventoryAdjResult> InventoryAdjSearch(
            DateTime? fromDate,
            DateTime? toDate,
            int? storeId,
            int? status,
            string adjNo,
            string productCode,
            string barcode,
            int pageIndex,
            int pageSize,
            out int totalRecord
            );

        IList<InventoryAdjLineResult> GetInventoryAdjDetail(int adjId);

        InventoryAdjResult GetInventoryAdj(int? adjId);

        IList<IVM_Barcode> BarcodeAjax(string code);

        IVM_Barcode GetBarcodeById(int id);

        int EditInventoryAdj(InventoryAdjResult obj);
    }
}
