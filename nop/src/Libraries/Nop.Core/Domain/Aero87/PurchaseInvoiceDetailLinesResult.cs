using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Aero87
{
    public class PurchaseInvoiceDetailLinesResult : BaseEntity
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string AttributeDesc { get; set; }
        public string Barcode { get; set; }
        public int BarcodeId { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public int SaleOrderId { get; set; }
        public decimal Amount { get; set; }
        public string PictureUrl { get; set; }
    }
}
