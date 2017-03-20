using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zit.BusinessObjects.SqlResultModel
{
    public class usp_GetPOSSaleOrderByOrderNo
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public int StoreId { get; set; }
        public string OrderNo { get; set; }
        public string Desc { get; set; }
        public string Mobile { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal ShippingFee { get; set; }
        public string RefNo { get; set; }
        public int ChanelId { get; set; }
        public decimal Amount { get; set; }
        public decimal ReturnMoney { get; set; }
        public decimal ReceiveMoney { get; set; }
        public string CreatedUser { get; set; }
        public Nullable<int> CarrierId { get; set; }
        public Nullable<int> ObjId { get; set; }
        public decimal AeroShippingFee { get; set; }
        public string Users { get; set; }
        public bool IsMasterCard { get; set; }
        public int SourceId { get; set; }
        public int? ReturnReasonId { get; set; }
        public string ShippingCode { get; set; }
        public bool IsPaid { get; set; }        
        public Nullable<DateTime> PaymentDate { get; set; }
    }
}
