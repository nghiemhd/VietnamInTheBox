using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Admin.Models.Aero87
{
    public class SaleOrderApproveModel
    {
        public int OrderId { get; set; }
        public int DebitAcctId { get; set; }
        public int? ObjId { get; set; }
        public int? FeeAcctId { get; set; }
        public decimal? FeeAmount { get; set; }
        public string Desc { get; set; }
    }
}