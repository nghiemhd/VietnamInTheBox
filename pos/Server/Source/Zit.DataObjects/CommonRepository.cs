using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public class CommonRepository : EFRepository<object>, ICommonRepository
    {
        public CommonRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public string GetPOSNextNumber(DateTime date)
        {
            var pTranType = new SqlParameter("TranType", "PS");
            var pTranDate = new SqlParameter("TranDate", date);
            return ExecQuery<string>("usp_Get_Next_Number @TranType, @TranDate", pTranType, pTranDate).Single();
        }

        public string GetITNextNumber(DateTime date)
        {
            var pTranType = new SqlParameter("TranType", "IT");
            var pTranDate = new SqlParameter("TranDate", date);
            return ExecQuery<string>("usp_Get_Next_Number @TranType, @TranDate", pTranType, pTranDate).Single();
        }
    }
}
