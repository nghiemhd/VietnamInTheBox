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
    public interface IIVMTransactionRepository : IRepository<IVM_Transaction>
    {
        bool CreateIVMTranByPOS(int orderId, int typeId);
        bool CreateIVMTranByIT(int transferId, int typeId, string user);
    }
}
