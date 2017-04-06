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
    public class IVMTransactionRepository : EFRepository<IVM_Transaction>, IIVMTransactionRepository
    {
        public IVMTransactionRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public bool CreateIVMTranByPOS(int orderId, int typeId)
        {
            var pOrderId = new SqlParameter("ordeId", SqlDbType.Int) { Value = orderId };
            var pTypeId = new SqlParameter("typeId", SqlDbType.Int) { Value = typeId };

            return ExecQuery<int>("[usp_CreateIVMTranByPOS] @ordeId,@typeId", pOrderId,pTypeId).FirstOrDefault() == 0;
        }

        public bool CreateIVMTranByIT(int transferId, int typeId, string user)
        {
            var pTransferId = new SqlParameter("transferId", SqlDbType.Int) { Value = transferId };
            var pTypeId = new SqlParameter("typeId", SqlDbType.Int) { Value = typeId };
            var puser = new SqlParameter("user", SqlDbType.VarChar) { Value = (((object)user) ?? DBNull.Value) };

            return ExecQuery<int>("[usp_CreateIVMTranByIT] @transferId,@typeId,@user", pTransferId, pTypeId, puser).FirstOrDefault() == 0;
        }
        
    }
}
