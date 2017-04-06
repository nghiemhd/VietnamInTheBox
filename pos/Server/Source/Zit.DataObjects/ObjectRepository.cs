using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.Core.Repository;
using Zit.BusinessObjects;

namespace Zit.DataObjects
{
    public class ObjectRepository : EFRepository<CF_Obj>, IObjectRepository
    {
        public ObjectRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public List<CF_Obj> GetAll()
        {
            return RepositoryQuery.ToList();
        }
    }
}
