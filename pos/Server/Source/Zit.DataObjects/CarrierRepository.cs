using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.Core.Repository;
using Zit.BusinessObjects;

namespace Zit.DataObjects
{
    public class CarrierRepository : EFRepository<CF_Carrier>, ICarrierRepository
    {
        public CarrierRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public List<CF_Carrier> GetAll()
        {
            return RepositoryQuery.ToList();
        }
    }
}
