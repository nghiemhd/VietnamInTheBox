using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using Zit.BusinessObjects;
using Zit.BusinessObjects.BusinessModels;
using Zit.BusinessObjects.SqlResultModel;
using Zit.DataTransferObjects;
namespace Zit.ServiceLibs
{
    [ServiceContract]
    public interface IZitServices : IConfigService
    {
        #region Authenticate
        
        [OperationContract]
        Response<UserContext> Login(int storeId, string userName, string password, string appKey);
        [OperationContract]
        Response<UserContext> CheckAndLogin(int storeId, string userName, string password, string appKey);

        #endregion

        #region Product

        #endregion

        #region POS

        [OperationContract]
        Response<POSSaleOrderModel> GetPOSSaleOrderByOrderNo(string orderNo);
        [OperationContract]
        Response<POSSaleOrderDetailModel> CreatePOSOrderDetailByBarcode(string barcode);
        [OperationContract]
        Response<POSSaleOrderModel> CreateNewPOSOrder();
        [OperationContract]
        Response<POSSaleOrderModel> SavePOSOrder(POSSaleOrderModel order);
        [OperationContract]
        Response SavePOSInfo(POSSaleOrderModel order);
        [OperationContract]
        Response<List<CF_Obj>> GetAllObj();
        [OperationContract]
        Response<List<CF_Carrier>> GetAllCarrier();
        [OperationContract]
        Response UpdatePrintCount(int orderId);

        #endregion

        #region Inventory

        [OperationContract]
        Response<InventoryTransferModel> CreateNewTransfer();
        [OperationContract]
        Response<InvenrotyTransferDetailModel> CreateITDetailByBarcode(string barcode);
        [OperationContract]
        Response<InventoryTransferModel> SaveInvTransfer(InventoryTransferModel transfer);
        [OperationContract]
        Response<InventoryTransferModel> GetInvTransferByTransferNo(string transferNo);
        [OperationContract]
        Response<List<CF_Store>> GetAllStore();

        #endregion

        #region HR

        [OperationContract]
        Response UserCheckTime(string userName, string password);
        [OperationContract]
        Response<string> GetUserByName(string userName);
        [OperationContract]
        Response UpdateUser(string userName, string fullName, string password, bool isActive);

        #endregion

        #region Common

        [OperationContract]
        Response<List<CF_SaleReturnReason>> GetAllSaleReturnReason();
        [OperationContract]
        Response<List<CF_SaleSource>> GetAllSaleSource();

        #endregion
    }
}