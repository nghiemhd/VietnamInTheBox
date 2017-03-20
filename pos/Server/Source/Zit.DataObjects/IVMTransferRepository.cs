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
    public class IVMTransferRepository : EFRepository<IVM_InvTransfer>, IIVMTransferRepository
    {
        public IVMTransferRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public usp_GetInvTransferByTransferNo GetByTransferNo(string transferNo)
        {
            if (string.IsNullOrWhiteSpace(transferNo)) return null;

            var pTransferNo = new SqlParameter("transferNo", SqlDbType.VarChar) { Value = transferNo };

            return ExecQuery<usp_GetInvTransferByTransferNo>("usp_GetInvTransferByTransferNo @transferNo", pTransferNo).FirstOrDefault();
        }

    }
}
