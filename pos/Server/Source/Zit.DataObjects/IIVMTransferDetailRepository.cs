using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.BusinessObjects;
using Zit.BusinessObjects.SqlResultModel;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public interface IIVMTransferDetailRepository : IRepository<IVM_InvTransferDetail>
    {
        usp_CreateITDetailByBarcode CreateITDetailByBarcode(string barcode);
        IEnumerable<usp_GetInvTransferDetailByTransferId> GetByTransferId(int transferId);
    }
}
