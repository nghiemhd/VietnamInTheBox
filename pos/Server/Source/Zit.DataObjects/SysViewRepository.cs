using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public class SysViewRepository : EFRepository<SYS_View>, ISysViewRepository
    {
        public SysViewRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public IEnumerable<SYS_View> GetAll()
        {
            return base.RepositoryQuery.AsEnumerable();
        }   
    }

    
}
