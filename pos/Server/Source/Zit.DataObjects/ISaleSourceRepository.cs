using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public interface ISaleSourceRepository : IRepository<CF_SaleSource>
    {
        List<CF_SaleSource> GetAll();
    }
}
