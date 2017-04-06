using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.BusinessObjects.BusinessModels;
using Zit.Core;

namespace Zit.BusinessLogic
{
    public interface IInventoryBusiness : IBusiness
    {
        void CreatePOSTransaction(int orderId);

        void CreateITTransaction(int transferId, string user);

        InventoryTransferModel CreateNewTransfer();

        InvenrotyTransferDetailModel CreateITDetailByBarcode(string barcode);

        InventoryTransferModel GetInvTransferByTransferNo(string transferNo);

        InventoryTransferModel SaveInvTransfer(InventoryTransferModel inv);
    }
}
