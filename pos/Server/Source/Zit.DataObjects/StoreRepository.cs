using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.Core.Repository;
using Zit.BusinessObjects;

namespace Zit.DataObjects
{
    public class StoreRepository :EFRepository<CF_Store>,IStoreRepository
    {
        public StoreRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public List<CF_Store> GetAll()
        {
            return RepositoryQuery.ToList();
        }
    }
}
