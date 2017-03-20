using Sendo.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.ServiceModel;
using System.Web;
using Zit.BusinessLogic;
using Zit.BusinessObjects;
using Zit.BusinessObjects.BusinessModels;
using Zit.BusinessObjects.SqlResultModel;
using Zit.Core;
using Zit.DataTransferObjects;
using Zit.EntLib.Extensions;
using Zit.Security;

namespace Zit.ServiceLibs
{
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Multiple,InstanceContextMode=InstanceContextMode.PerCall)]
    public partial class ZitServices : IZitServices
    {
        #region Authenticate

        public Response<UserContext> Login(int storeId, string userName, string password, string appKey)
        {
            BusinessProcess.Current.Process(bp => IoC.Get<IAuthenticateBusiness>().Login(storeId, userName, password, appKey));
            if (BusinessProcess.Current.HasError)
            {
                return BusinessProcess.Current.ToResponse(null as UserContext);
            }
            else
            {
                return Response.FromData(WorkContext.UserContext);
            }
        }

        public Response<UserContext> CheckAndLogin(int storeId, string userName, string password, string appKey)
        {
            var ss = ZitSession.Current;
            if (ss.IsAuthenticated) return Response.FromData(WorkContext.UserContext);
            BusinessProcess.Current.Process(bp => IoC.Get<IAuthenticateBusiness>().Login(storeId, userName, password, appKey));
            if (BusinessProcess.Current.HasError)
            {
                return BusinessProcess.Current.ToResponse(null as UserContext);
            }
            else
            {
                return Response.FromData(WorkContext.UserContext);
            }
        }

        #endregion

        #region Product

        #endregion

        #region POS

        public Response<POSSaleOrderModel> GetPOSSaleOrderByOrderNo(string orderNo)
        {
            var bo = IoC.Get<ISaleOrderBusiness>();
            return BusinessProcess.Current.ToResponse(bo.GetPOSSaleOrderByOrderNo(orderNo));
        }

        public Response<POSSaleOrderDetailModel> CreatePOSOrderDetailByBarcode(string barcode)
        {
            var bo = IoC.Get<ISaleOrderBusiness>();
            return BusinessProcess.Current.ToResponse(bo.CreatePOSOrderDetailByBarcode(barcode));
        }

        public Response<POSSaleOrderModel> CreateNewPOSOrder()
        {
            var bo = IoC.Get<ISaleOrderBusiness>();
            var rs = bo.CreateNewPOSOrder();
            return BusinessProcess.Current.ToResponse(rs);
        }

        public Response<POSSaleOrderModel> SavePOSOrder(POSSaleOrderModel order)
        {
            var bo = IoC.Get<ISaleOrderBusiness>();
            var rs = bo.SavePOSOrder(order);
            return BusinessProcess.Current.ToResponse(rs);
        }

        public Response SavePOSInfo(POSSaleOrderModel order)
        {
            var bo = IoC.Get<ISaleOrderBusiness>();
            bo.SavePOSInfo(order);
            return BusinessProcess.Current.ToResponse();
        }

        public Response UpdatePrintCount(int orderId)
        {
            var bo = IoC.Get<ISaleOrderBusiness>();
            bo.UpdatePrintCount(orderId);
            return BusinessProcess.Current.ToResponse();
        }

        public Response<List<CF_Obj>> GetAllObj()
        {
            var bo = IoC.Get<IObjectBusiness>();
            var rs = bo.GetAllObj();
            return BusinessProcess.Current.ToResponse(rs);
        }

        public Response<List<CF_Carrier>> GetAllCarrier()
        {
            var bo = IoC.Get<IObjectBusiness>();
            var rs = bo.GetAllCarrier();
            return BusinessProcess.Current.ToResponse(rs);
        }

        #endregion

        #region Inventory

        public Response<InventoryTransferModel> CreateNewTransfer()
        {
            var bo = IoC.Get<IInventoryBusiness>();
            var rs = bo.CreateNewTransfer();
            return BusinessProcess.Current.ToResponse(rs);
        }

        public Response<InvenrotyTransferDetailModel> CreateITDetailByBarcode(string barcode)
        {
            var bo = IoC.Get<IInventoryBusiness>();
            var rs = bo.CreateITDetailByBarcode(barcode);
            return BusinessProcess.Current.ToResponse(rs);
        }

        public Response<InventoryTransferModel> GetInvTransferByTransferNo(string transferNo)
        {
            var bo = IoC.Get<IInventoryBusiness>();
            return BusinessProcess.Current.ToResponse(bo.GetInvTransferByTransferNo(transferNo));
        }

        public Response<InventoryTransferModel> SaveInvTransfer(InventoryTransferModel transfer)
        {
            var bo = IoC.Get<IInventoryBusiness>();
            var rs = bo.SaveInvTransfer(transfer);
            return BusinessProcess.Current.ToResponse(rs);
        }

        public Response<List<CF_Store>> GetAllStore()
        {
            var bo = IoC.Get<IStoreBusiness>();
            var rs = bo.GetAllStore();
            return BusinessProcess.Current.ToResponse(rs);
        }

        #endregion

        #region HR

        public Response UserCheckTime(string userName, string password)
        {
            IoC.Get<IHrBusiness>().UserCheckTime(userName, password);
            return BusinessProcess.Current.ToResponse();
        }

        public Response<string> GetUserByName(string userName)
        {
            var rp = IoC.Get<IHrBusiness>().GetUserByName(userName);
            return BusinessProcess.Current.ToResponse(rp);
        }

        public Response UpdateUser(string userName, string fullName, string password, bool isActive)
        {
            IoC.Get<IHrBusiness>().UpdateUser(userName,fullName,password,isActive);
            return BusinessProcess.Current.ToResponse();
        }

        #endregion

        #region Common

        public Response<List<CF_SaleReturnReason>> GetAllSaleReturnReason()
        {
            var bo = IoC.Get<ICommonBusiness>();
            var rs = bo.GetAllSaleReturnReason();
            return BusinessProcess.Current.ToResponse(rs);
        }

        public Response<List<CF_SaleSource>> GetAllSaleSource()
        {
            var bo = IoC.Get<ICommonBusiness>();
            var rs = bo.GetAllSaleSource();
            return BusinessProcess.Current.ToResponse(rs);
        }

        #endregion
    }
}