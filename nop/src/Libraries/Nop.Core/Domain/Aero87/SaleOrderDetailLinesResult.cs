using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Aero87
{
    public class SaleOrderDetailLinesResult : BaseEntity
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string AttributeDesc { get; set; }
        public string Barcode { get; set; }
        public int BarcodeId { get; set; }
        public decimal LineAmount { get; set; }
        public int LineDiscount { get; set; }
        public int Qty { get; set; }
        public int SaleOrderId { get; set; }
        public decimal SellUnitPrice { get; set; }
        public int Seq { get; set; }
        public string PictureUrl { get; set; }

        public decimal CostPrice { get; set; }
        public decimal CostAmount { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public string CreatedUser { get; set; }
    }
}
