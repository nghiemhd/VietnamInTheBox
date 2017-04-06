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
    public interface IIVMTransferRepository : IRepository<IVM_InvTransfer>
    {
        usp_GetInvTransferByTransferNo GetByTransferNo(string transferNo);
    }
}
