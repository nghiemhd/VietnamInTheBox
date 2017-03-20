using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zit.BusinessObjects.SqlResultModel
{
    public class usp_GetInvTransferByTransferNo
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int ToStoreId { get; set; }
        public string TransferNo { get; set; }
        public string Desc { get; set; }
        public DateTime TransferDate { get; set; }
        public string CreatedUser { get; set; }
    }
}
