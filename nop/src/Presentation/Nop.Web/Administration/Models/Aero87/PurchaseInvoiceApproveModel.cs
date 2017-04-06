using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Admin.Models.Aero87
{
    public class PurchaseInvoiceApproveModel
    {
        public int InvoiceId { get; set; }
        public int CreditAcctId { get; set; }
        public int? ObjId { get; set; }
        public string Desc { get; set; }
    }
}