using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.Core.Repository;
using Zit.BusinessObjects;

namespace Zit.DataObjects
{
    public class SaleSourceRepository : EFRepository<CF_SaleSource>, ISaleSourceRepository
    {
        public SaleSourceRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public List<CF_SaleSource> GetAll()
        {
            return RepositoryQuery.ToList();
        }
    }
}
