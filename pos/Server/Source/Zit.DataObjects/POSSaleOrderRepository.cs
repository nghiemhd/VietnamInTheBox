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
    public class POSSaleOrderRepository : EFRepository<POS_SaleOrder>, IPOSSaleOrderRepository
    {
        public POSSaleOrderRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public usp_GetPOSSaleOrderByOrderNo GetByOrderNo(string orderNo)
        {
            if (string.IsNullOrWhiteSpace(orderNo)) return null;

            var pOrderNo = new SqlParameter("orderNo", SqlDbType.VarChar) { Value = orderNo };

            return ExecQuery<usp_GetPOSSaleOrderByOrderNo>("usp_GetPOSSaleOrderByOrderNo @orderNo", pOrderNo).FirstOrDefault();
        }
    }
}
