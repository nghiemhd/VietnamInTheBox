using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zit.BusinessObjects.Enums
{
    public enum InventoryTranType : int
    {
        PI = 1, //Purchase Invoice
        PS = 2,  //POS Saler Order
        IT = 5   //Transfer
    }
}
