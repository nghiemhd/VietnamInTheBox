using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Aero87
{
    public class WarehouseCountLineResult : BaseEntity
    {
        public string AttributeDesc { get; set; }
        public string Barcode { get; set; }
        public int Qty { get; set; }
        public decimal CostAmount { get; set; }
        public decimal CostPrice
        {
            get
            {
                if (Qty != 0)
                    return CostAmount / Qty;
                else return 0;
            }
        }
    }
}
