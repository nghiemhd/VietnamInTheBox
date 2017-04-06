using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Zit.BusinessObjects;
using Zit.BusinessObjects.BusinessModels;
using Zit.BusinessObjects.Enums;
using Zit.BusinessObjects.SqlResultModel;
using Zit.Core;
using Zit.Core.Repository;
using Zit.DataObjects;
using Zit.DataTransferObjects;
using Zit.Entity;
using Zit.EntLib.Extensions;
using System.Transactions;
using System.Security.Permissions;
using Zit.BusinessObjects.Constants;
using Zit.Security;
using Sendo.BusinessLogic;

namespace Zit.BusinessLogic
{
    public class SaleOrderBusiness : BusinessBase, ISaleOrderBusiness
    {
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = Functions.PS)]
        public POSSaleOrderModel GetPOSSaleOrderByOrderNo(string orderNo)
        {
            if (string.IsNullOrWhiteSpace(orderNo))
            {
                this.AddError("Mã đơn hàng không hợp lệ");
                return null;
            }

            var rpOrder = IoC.Get<IPOSSaleOrderRepository>();

            var order = rpOrder.GetByOrderNo(orderNo);

            if (order == null)
            {
                this.AddError("Không tìm thấy đơn hàng");
                return null;
            }

            if (order.StoreId != WorkContext.UserContext.StoreId)
            {
                this.AddError("Đơn hàng không thuộc cửa hàng hiện tại");
                return null;
            }

            var rpOrderDetail = IoC.Get<IPOSSaleOrderDetailRepository>();
            var details = rpOrderDetail.GetBySaleOrderId(order.Id);

            var dataReturn = Mapper.Map<POSSaleOrderModel>(order);

            dataReturn.Detail = new ZitObservableCollection<POSSaleOrderDetailModel>(Mapper.Map<IEnumerable<POSSaleOrderDetailModel>>(details));

            dataReturn.RequestDataDate = DateTime.Now;

            return dataReturn;
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = Functions.PS)]
        public POSSaleOrderDetailModel CreatePOSOrderDetailByBarcode(string barcode)
        {
            var rp = IoC.Get<IPOSSaleOrderDetailRepository>();
            return Mapper.Map<POSSaleOrderDetailModel>(rp.CreatePOSDetailByBarcode(barcode));
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = Functions.PS)]
        public POSSaleOrderModel CreateNewPOSOrder()
        {
            var rpGen = IoC.Get<ICommonRepository>();

            var now = DateTime.Now;

            var order = new POSSaleOrderModel()
            {
                StoreId = WorkContext.UserContext.StoreId,
                OrderDate = now,
                DiscountPercent = 0,
                OrderNo = rpGen.GetPOSNextNumber(now),
                RefNo = null,
                Id = 0,
                ChanelId = SaleChanel.Shop,
                Detail = new ZitObservableCollection<POSSaleOrderDetailModel>(),
                CreatedUser = ZitSession.Current.Principal.Identity.Name,
                ReturnReasonId = null,
                SourceId = 100000
            };

            return order;
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = Functions.PS)]
        public POSSaleOrderModel SavePOSOrder(POSSaleOrderModel order)
        {
            if (order == null
                || order.Detail == null
                || !order.Detail.Any())
            {
                this.AddError("Dữ liệu không hợp lệ");
                return null;
            }

            var rpOrder = IoC.Get<IPOSSaleOrderRepository>();
            var rpDetail = IoC.Get<IPOSSaleOrderDetailRepository>();

            SaleChanel cn = order.ChanelId;

            var master = Mapper.Map<POS_SaleOrder>(order);

            //Check Exist
            var existOrder = rpOrder.GetByOrderNo(order.OrderNo);
            if (existOrder != null)
            {
                if (master.ReturnReasonId == null)
                {
                    this.AddError("Lý do trả hàng không thể rổng");
                    return null;
                }
                //return mode
                var rpGen = IoC.Get<ICommonRepository>();
                var now = DateTime.Now;
                master.RefNo = existOrder.OrderNo;
                master.OrderNo = rpGen.GetPOSNextNumber(now);
                master.OrderDate = now;
                master.Users = existOrder.Users;
                master.IsPaid = existOrder.IsPaid;
                master.PaymentDate = existOrder.PaymentDate;

                order.Detail.ToList().ForEach(item =>
                {
                    if (!item.IsReturn)
                        order.Detail.Remove(item);
                });
            }
            else
            {
                master.ReturnReasonId = null;
            }

            if (master.SourceId == 0)
            {
                master.SourceId = 100000;
            }

            var detail = Mapper.Map<List<POS_SaleOrderDetail>>(order.Detail);

            if (detail.Count < 1)
            {
                this.AddError("Đơn hàng phải có ít nhất một sản phẩm");
                return null;
            }

            if (string.IsNullOrWhiteSpace(master.OrderNo)
                || master.OrderDate < DateTime.Now.Date
                || cn == 0
                || ((cn == SaleChanel.Aero87 || cn == SaleChanel.Sendo) && string.IsNullOrWhiteSpace(master.RefNo))
                )
            {
                this.AddError("Dữ liệu không hợp lệ");
                return null;
            }

            if (cn == SaleChanel.NoiBo && master.ObjId == null)
            {
                this.AddError("Kênh nội bộ phải chỉ định đối tượng công nợ");
                return null;
            }

            if (
                (cn == SaleChanel.Aero87 || cn == SaleChanel.Phone) &&
                master.CarrierId == null
                )
            {
                this.AddError("Kênh bán hàng bắt buộc phải chọn nhà vận chuyển");
                return null;
            }

            if (cn != SaleChanel.Shop)
                master.IsMasterCard = false;
            else if(existOrder == null)
            {
                master.IsPaid = true;
                master.PaymentDate = DateTime.Now;
            }

            master.OrderDate = DateTime.Now;
            master.StoreId = WorkContext.UserContext.StoreId;
            master.Status = 1;
            master.PrintCount = 0;

            rpOrder.Add(master);

            foreach (var d in detail)
            {
                if (d.LineDiscount < 0
                    || d.Qty == 0
                    || d.SellUnitPrice < 0
                    )
                {
                    this.AddError("Dữ liệu không hợp lệ");
                    break;
                }
                else
                {
                    d.LineAmount = d.Qty * d.SellUnitPrice * (100 - d.LineDiscount) / 100;
                    d.POS_SaleOrder = master;
                    rpDetail.Add(d);
                }
            }

            master.SubTotal = detail.Sum(m => m.LineAmount);
            master.Amount = master.SubTotal - master.Discount + master.ShippingFee;

            using (TransactionScope scope = new TransactionScope())
            {
                IoC.Get<IUnitOfWork>().Commit();
                IoC.Get<IInventoryBusiness>().CreatePOSTransaction(master.Id);
                scope.Complete();
            }

            return GetPOSSaleOrderByOrderNo(master.OrderNo);
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = Functions.PS)]
        public void SavePOSInfo(POSSaleOrderModel order)
        {
            var rpOrder = IoC.Get<IPOSSaleOrderRepository>();

            var existOrder = rpOrder.GetByID(order.Id);

            if (existOrder == null)
            {
                this.AddError("Chứng từ không tồn tại");
                return;
            }

            if (existOrder.Amount != order.Amount || (existOrder.Status != 1 && existOrder.AeroShippingFee != order.AeroShippingFee))
            {
                this.AddError("Không cho phép sửa phí vận chuyển khi đơn hàng đã duyệt hoặc làm thay đổi tổng giá trị đơn hàng");
                return;
            }

            SaleChanel cn = (SaleChanel)existOrder.ChanelId;
            bool isUpdateShippingFee = false;

            if (cn == SaleChanel.Aero87 || cn == SaleChanel.Phone || cn == SaleChanel.Sendo)
            {
                if (existOrder.AeroShippingFee != order.AeroShippingFee)
                {
                    isUpdateShippingFee = true;
                }
            }

            existOrder.Desc = order.Desc;
            existOrder.Mobile = order.Mobile;
            existOrder.CustomerName = order.CustomerName;
            existOrder.SourceId = order.SourceId;
            existOrder.ReturnReasonId = order.ReturnReasonId;
            existOrder.ShippingCode = order.ShippingCode;
            if (existOrder.ChanelId == (int)SaleChanel.Shop && existOrder.Status == 1)
            {
                existOrder.IsMasterCard = order.IsMasterCard;
            }

            if (isUpdateShippingFee)
            {
                existOrder.AeroShippingFee = order.AeroShippingFee;
                rpOrder.Update(existOrder, m => m.CustomerName, m => m.Mobile, m => m.Desc
                    , m => m.AeroShippingFee, m => m.IsMasterCard,m => m.SourceId, m => m.ShippingCode
                    );
            }
            else
            {
                rpOrder.Update(existOrder, m => m.CustomerName, m => m.Mobile, m => m.Desc, m => m.IsMasterCard, m => m.SourceId , m => m.ShippingCode);
            }

            IoC.Get<IUnitOfWork>().Commit();
        }
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = Functions.PS)]
        public void UpdatePrintCount(int orderId)
        {
            var rpOrder = IoC.Get<IPOSSaleOrderRepository>();
            var order = rpOrder.GetByID(orderId);
            order.PrintCount += 1;
            rpOrder.Update(order, m => m.PrintCount);
            IoC.Get<IUnitOfWork>().Commit();
        }
    }
}
