using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.Core.Repository;
using Zit.BusinessObjects;

namespace Zit.DataObjects
{
    public class SaleReturnReasonRepository : EFRepository<CF_SaleReturnReason>, ISaleReturnReasonRepository
    {
        public SaleReturnReasonRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public List<CF_SaleReturnReason> GetAll()
        {
            return RepositoryQuery.ToList();
        }
    }
}
