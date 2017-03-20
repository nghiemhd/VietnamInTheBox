using AutoMapper;
using Sendo.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Zit.BusinessObjects;
using Zit.BusinessObjects.BusinessModels;
using Zit.BusinessObjects.Constants;
using Zit.BusinessObjects.Enums;
using Zit.Core;
using Zit.Core.Repository;
using Zit.DataObjects;
using Zit.Entity;
using Zit.EntLib.Extensions;
using Zit.Security;

namespace Zit.BusinessLogic
{
    public class InventoryBusiness : BusinessBase, IInventoryBusiness
    {
        /// <summary>
        /// In transaction
        /// </summary>
        /// <param name="order"></param>
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = Functions.PS)]
        public void CreatePOSTransaction(int orderId)
        {
            var rp = IoC.Get<IIVMTransactionRepository>();

            if (!rp.CreateIVMTranByPOS(orderId, (int)InventoryTranType.PS))
                throw new OperationCanceledException();
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = Functions.IT)]
        public void CreateITTransaction(int transferId,string user)
        {
            var rp = IoC.Get<IIVMTransactionRepository>();

            if (!rp.CreateIVMTranByIT(transferId, (int)InventoryTranType.IT , user))
                throw new OperationCanceledException();
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = Functions.IT)]
        public InventoryTransferModel CreateNewTransfer()
        {
            var rpGen = IoC.Get<ICommonRepository>();

            var now = DateTime.Now;

            var order = new InventoryTransferModel()
            {
                StoreId = WorkContext.UserContext.StoreId,
                TransferDate = now,
                TransferNo = rpGen.GetITNextNumber(now),                
                Id = 0,
                Detail = new ZitObservableCollection<InvenrotyTransferDetailModel>(),
                CreatedUser = ZitSession.Current.Principal.Identity.Name
            };

            return order;
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = Functions.IT)]
        public InvenrotyTransferDetailModel CreateITDetailByBarcode(string barcode)
        {
            var rp = IoC.Get<IIVMTransferDetailRepository>();
            return Mapper.Map<InvenrotyTransferDetailModel>(rp.CreateITDetailByBarcode(barcode));
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = Functions.IT)]
        public InventoryTransferModel SaveInvTransfer(InventoryTransferModel inv)
        {
            if (inv == null
                || inv.Detail == null
                || !inv.Detail.Any())
            {
                this.AddError("Dữ liệu không hợp lệ");
                return null;
            }

            var rpInv = IoC.Get<IIVMTransferRepository>();
            var rpDetail = IoC.Get<IIVMTransferDetailRepository>();

            var master = Mapper.Map<IVM_InvTransfer>(inv);

            //Check Exist
            var existOrder = rpInv.GetByTransferNo(inv.TransferNo);
            if (existOrder != null)
            {
                this.AddError("Vận đơn đã tồn tại");
                return null;
            }

            var detail = Mapper.Map<List<IVM_InvTransferDetail>>(inv.Detail);

            if (detail.Count < 1)
            {
                this.AddError("Đơn hàng phải có ít nhất một sản phẩm");
                return null;
            }


            if (string.IsNullOrWhiteSpace(master.TransferNo)
                || master.TransferDate < DateTime.Now.Date
                )
            {
                this.AddError("Dữ liệu không hợp lệ");
                return null;
            }

            if (master.ToStoreId == 0 || master.ToStoreId == master.StoreId)
            {
                this.AddError("Dữ liệu không hợp lệ");
                return null;
            }

            master.TransferDate = DateTime.Now;
            master.StoreId = WorkContext.UserContext.StoreId;
            master.Status = 1;

            rpInv.Add(master);

            foreach (var d in detail)
            {
                d.IVM_InvTransfer = master;
                rpDetail.Add(d);
            }

            master.Qty = detail.Sum(m => m.Qty);

            using (TransactionScope scope = new TransactionScope())
            {
                IoC.Get<IUnitOfWork>().Commit();
                IoC.Get<IInventoryBusiness>().CreateITTransaction(master.Id,WorkContext.UserContext.UserName);
                scope.Complete();
            }

            return GetInvTransferByTransferNo(master.TransferNo);
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = Functions.IT)]
        public InventoryTransferModel GetInvTransferByTransferNo(string transferNo)
        {
            if (string.IsNullOrWhiteSpace(transferNo))
            {
                this.AddError("Mã vận đơn không hợp lệ");
                return null;
            }

            var rpInv = IoC.Get<IIVMTransferRepository>();

            var inv = rpInv.GetByTransferNo(transferNo);

            if (inv == null)
            {
                this.AddError("Không tìm thấy vận đơn");
                return null;
            }

            if (inv.StoreId != WorkContext.UserContext.StoreId)
            {
                this.AddError("Đơn hàng không thuộc cửa hàng hiện tại");
                return null;
            }

            var rpDetail = IoC.Get<IIVMTransferDetailRepository>();
            var details = rpDetail.GetByTransferId(inv.Id);

            var dataReturn = Mapper.Map<InventoryTransferModel>(inv);

            dataReturn.Detail = new ZitObservableCollection<InvenrotyTransferDetailModel>(Mapper.Map<IEnumerable<InvenrotyTransferDetailModel>>(details));

            return dataReturn;
        }
    }
}
