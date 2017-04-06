using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.BusinessObjects;
using Zit.BusinessObjects.SqlResultModel;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public class IVMTransferDetailRepository : EFRepository<IVM_InvTransferDetail>, IIVMTransferDetailRepository
    {
        public IVMTransferDetailRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public IEnumerable<usp_GetInvTransferDetailByTransferId> GetByTransferId(int transferId)
        {
            var pTransferId = new SqlParameter("transferId", SqlDbType.Int) { Value = transferId };

            return ExecQuery<usp_GetInvTransferDetailByTransferId>("usp_GetInvTransferDetailByTransferId @transferId", pTransferId);
        }

        public usp_CreateITDetailByBarcode CreateITDetailByBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode)) return null;

            var pBarcode = new SqlParameter("barcode", SqlDbType.VarChar) { Value = barcode };

            return ExecQuery<usp_CreateITDetailByBarcode>("usp_CreateITDetailByBarcode @barcode", pBarcode).FirstOrDefault();
        }
    }
}
