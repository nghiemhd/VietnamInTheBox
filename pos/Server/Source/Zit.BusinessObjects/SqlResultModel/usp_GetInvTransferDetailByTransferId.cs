using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zit.BusinessObjects.SqlResultModel
{
    public class usp_GetInvTransferDetailByTransferId
    {
        public int Id {get;set;}
	    public int ProductId {get;set;}
	    public string Name {get;set;}
	    public string AttributeDesc {get;set;}
        public int BarcodeId { get; set; }
	    public string Barcode {get;set;}
	    public int Qty {get;set;}
	    public int TransferId {get;set;}
        public int Seq { get; set; }
    }
}
