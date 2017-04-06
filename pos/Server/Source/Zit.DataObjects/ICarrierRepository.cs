using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public interface ICarrierRepository : IRepository<CF_Carrier>
    {
        List<CF_Carrier> GetAll();
    }
}
