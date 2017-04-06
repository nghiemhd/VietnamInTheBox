using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public interface IStoreRepository : IRepository<CF_Store>
    {
        List<CF_Store> GetAll();
    }
}
