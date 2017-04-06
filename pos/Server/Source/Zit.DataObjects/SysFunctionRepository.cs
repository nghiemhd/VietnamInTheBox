using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;
using Zit.Core;
using Zit.Core.Repository;
using Zit.Utils;

namespace Zit.DataObjects
{
    public class SysFunctionRepository : EFRepository<SYS_Function>, ISysFunctionRepository
    {
        public SysFunctionRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public IEnumerable<SYS_Function> GetAll()
        {
            return base.RepositoryQuery.AsEnumerable();
        }
    }
}
