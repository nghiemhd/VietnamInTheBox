using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.BusinessObjects.BusinessModels;
using Zit.Core;
using Zit.DataTransferObjects;

namespace Zit.BusinessLogic
{
    public interface ISaleOrderBusiness : IBusiness
    {
        POSSaleOrderDetailModel CreatePOSOrderDetailByBarcode(string barcode);
        POSSaleOrderModel GetPOSSaleOrderByOrderNo(string orderNo);
        POSSaleOrderModel CreateNewPOSOrder();
        POSSaleOrderModel SavePOSOrder(POSSaleOrderModel order);
        void SavePOSInfo(POSSaleOrderModel order);
        void UpdatePrintCount(int orderId);
    }
}
